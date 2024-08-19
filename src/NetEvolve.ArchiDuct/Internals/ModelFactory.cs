namespace NetEvolve.ArchiDuct.Internals;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Models;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Members;
using NetEvolve.ArchiDuct.Models.Types;
using static System.StringComparison;
using static ICSharpCode.Decompiler.TypeSystem.Accessibility;

internal static class ModelFactory
{
    private static readonly string[] _fileTypeSeparator = ["__"];

    private static readonly string[] _ignoredElements =
    [
        DocumentationXmlPropertyConstants.Example,
        DocumentationXmlPropertyConstants.Remarks,
        DocumentationXmlPropertyConstants.Returns,
        DocumentationXmlPropertyConstants.SeeAlso,
        DocumentationXmlPropertyConstants.Summary,
    ];

    private static readonly ConcurrentDictionary<
        IModule,
        IDocumentationProvider?
    > _documentationProviders = new(new ModuleEqualityComparer());

    public static bool TryGetDocumentationProvider(
        IModule module,
        [NotNullWhen(true)] out IDocumentationProvider? documentationProvider
    )
    {
        documentationProvider = _documentationProviders.GetOrAdd(
            module,
            m =>
            {
                XmlDocumentationProvider? provider = null;
                if (m.MetadataFile is not null && File.Exists(m.MetadataFile.FileName))
                {
                    provider = XmlDocLoader.LoadDocumentation(m.MetadataFile);
                }

                if (provider is null)
                {
                    provider = XmlDocLoader.MscorlibDocumentation;
                }

                return provider;
            }
        );

        return documentationProvider is not null;
    }

    public static ModelMemberBase CreateModelMemberType(
        IMember member,
        ModelTypeBase parentModel,
        XElement? documentation,
        ITypeResolveContext resolver
    )
    {
        ModelMemberBase model = member switch
        {
            IField field when member.DeclaringType.Kind == TypeKind.Enum => new ModelEnumMember(
                field,
                parentModel,
                documentation
            ),
            IField field => new ModelField(field, parentModel, documentation),
            IProperty property when property.IsExplicitInterfaceImplementation =>
                new ModelExplicitProperty(property, parentModel, documentation),
            IProperty property when property.IsIndexer => new ModelIndexer(
                property,
                parentModel,
                documentation
            ),
            IProperty property => new ModelProperty(property, parentModel, documentation),
            IMethod method when method.IsExplicitInterfaceImplementation => new ModelExplicitMethod(
                method,
                parentModel,
                documentation
            ),
            IMethod method when method.IsConstructor && method.IsStatic =>
                new ModelStaticConstructor(method, parentModel, documentation),
            IMethod method when method.IsConstructor => new ModelConstructor(
                method,
                parentModel,
                documentation
            ),
            IMethod method when method.IsOperator => new ModelOperator(
                method,
                parentModel,
                documentation
            ),
            IMethod method when method.IsDestructor => new ModelDestructor(
                method,
                parentModel,
                documentation
            ),
            IMethod method when method.Name.Equals("Deconstruct", Ordinal) =>
                new ModelDeconstructor(method, parentModel, documentation),
            IMethod method when method.IsExtensionMethod => new ModelExtensionMethod(
                method,
                parentModel,
                documentation
            ),
            IMethod method => new ModelMethod(method, parentModel, documentation),
            IEvent @event when @event.IsExplicitInterfaceImplementation => new ModelExplicitEvent(
                @event,
                parentModel,
                documentation
            ),
            IEvent @event => new ModelEvent(@event, parentModel, documentation),
            _ => throw new InvalidOperationException(),
        };
        model.Modifiers = MapModifiers(member);
        model.Attributes = MapAttributes(member, resolver);

        return model;
    }

    public static ModelTypeBase? CreateModelType(
        ITypeDefinition typeDefinition,
        ModelEntityBase parentEntity,
        XElement? documentation,
        ITypeResolveContext resolver
    )
    {
#pragma warning disable IDE0072 // Add missing cases
        ModelTypeBase? model = typeDefinition.Kind switch
        {
            TypeKind.Class => new ModelClass(typeDefinition, parentEntity, documentation),
            TypeKind.Interface => new ModelInterface(typeDefinition, parentEntity, documentation),
            TypeKind.Struct => new ModelStruct(typeDefinition, parentEntity, documentation),
            TypeKind.Delegate => new ModelDelegate(typeDefinition, parentEntity, documentation),
            TypeKind.Enum => new ModelEnum(typeDefinition, parentEntity, documentation),

            TypeKind.Void => null,
            _ => throw new InvalidOperationException(),
        };
#pragma warning restore IDE0072 // Add missing cases

        if (model is null)
        {
            return model;
        }

        model.Accessibility = MapAccessibility(typeDefinition);

        model.BaseTypes = typeDefinition.GetAllBaseTypeIds();
        model.Modifiers = MapModifiers(typeDefinition);
        model.Implementations = typeDefinition.GetAllImplementationIds();
        model.InheritedMembers = typeDefinition.GetAllInheritedMemberIds();
        model.Attributes = MapAttributes(typeDefinition, resolver);

        return model;
    }

    private static ModelAccessibility MapAccessibility(ITypeDefinition typeDefinition)
    {
        if (HasFileAccessModifier(typeDefinition))
        {
            return ModelAccessibility.File;
        }

        return typeDefinition.Accessibility switch
        {
            Public => ModelAccessibility.Public,
            ProtectedAndInternal => ModelAccessibility.PrivateProtected,
            ProtectedOrInternal => ModelAccessibility.ProtectedInternal,
            Protected => ModelAccessibility.Protected,
            Internal => ModelAccessibility.Internal,
            Private => ModelAccessibility.Private,
            _ => ModelAccessibility.Internal,
        };
    }

    internal static HashSet<ModelAttribute> MapAttributes(
        IMember member,
        ITypeResolveContext resolver
    ) =>
        member
            .GetAttributes(true)
            .Aggregate(
                new HashSet<ModelAttribute>(),
                (set, attribute) =>
                {
                    var typeDefinition = attribute.AttributeType.GetDefinition()!;

                    if (typeDefinition is not null)
                    {
                        _ = set.Add(
                            new ModelAttribute(
                                attribute,
                                typeDefinition,
                                GetDocumentation(typeDefinition.GetIdString(), resolver)
                            )
                        );
                    }
                    return set;
                }
            );

    internal static HashSet<ModelAttribute> MapAttributes(
        ITypeDefinition typeDefinition,
        ITypeResolveContext resolver
    ) =>
        typeDefinition
            .GetAttributes(true)
            .Aggregate(
                new HashSet<ModelAttribute>(),
                (set, attribute) =>
                {
                    var attributeTypeDefinition = attribute.AttributeType.GetDefinition()!;

                    if (attributeTypeDefinition is not null)
                    {
                        _ = set.Add(
                            new ModelAttribute(
                                attribute,
                                attributeTypeDefinition,
                                GetDocumentation(typeDefinition.GetIdString(), resolver)
                            )
                        );
                    }
                    return set;
                }
            );

    internal static ModelReturn? GetReturnType(IMember member, bool? isRefReadonly = null)
    {
        if (member is IMethod constructor && constructor.IsConstructor)
        {
            return null;
        }

        var returnType = member.ReturnType;
        if (returnType.FullName == "System.Void")
        {
            return ModelReturn.Void;
        }

        var returnId = $"T:{returnType.ReflectionName}";
        if (returnType.Kind == TypeKind.TypeParameter)
        {
            if (
                member.MemberDefinition is IMethod method
                && method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal))
            )
            {
                returnId = $"T:{method.ReflectionName}.{returnType.Name}";
            }
            else
            {
                returnId = $"T:{member.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
            }
        }

        return new ModelReturn(
            returnId,
            returnType.Nullability == Nullability.Nullable,
            isRefReadonly
        );
    }

    internal static ModelReturn GetReturnType(IParameter parameter)
    {
        var returnType = parameter.Type;
        if (returnType.FullName == "System.Void")
        {
            return ModelReturn.Void;
        }

        var returnId = $"T:{returnType.ReflectionName}";
        if (returnType.Kind == TypeKind.TypeParameter && parameter.Owner is IMethod method)
        {
            if (method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal)))
            {
                returnId = $"T:{method.ReflectionName}.{returnType.Name}";
            }
            else
            {
                returnId = $"T:{method.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
            }
        }

        return new ModelReturn(returnId, returnType.Nullability == Nullability.Nullable);
    }

    internal static HashSet<ModelModifier> MapModifiers(IParameter parameter) =>
        GetModifiers(parameter).ToHashSet();

    private static IEnumerable<ModelModifier> GetModifiers(IParameter parameter)
    {
        if (parameter.IsIn)
        {
            yield return ModelModifier.In;
        }

        if (parameter.IsRef)
        {
            yield return ModelModifier.Ref;
        }

        if (parameter.IsOut)
        {
            yield return ModelModifier.Out;
        }

        if (parameter.IsParams)
        {
            yield return ModelModifier.Params;
        }

        if (
            parameter.Owner is IMethod method
            && (method.IsExtensionMethod || method.HasAttribute(KnownAttribute.Extension))
            && method.Parameters[0] == parameter
        )
        {
            yield return ModelModifier.This;
        }
    }

    private static IEnumerable<ModelModifier> GetModifiers(ITypeDefinition type)
    {
        if (type.Kind != TypeKind.Enum)
        {
            if (type.IsAbstract && type.Kind != TypeKind.Interface)
            {
                yield return ModelModifier.Abstract;
            }

            if (type.IsReadOnly)
            {
                yield return ModelModifier.ReadOnly;
            }

            if (type.IsSealed && type.Kind != TypeKind.Struct)
            {
                yield return ModelModifier.Sealed;
            }

            if (type.IsStatic)
            {
                yield return ModelModifier.Static;
            }

            if (type.IsByRefLike && type.Kind == TypeKind.Struct)
            {
                yield return ModelModifier.Ref;
            }

            // TODO: unsafe
        }
    }

    private static IEnumerable<ModelModifier> GetModifiers(IMember m)
    {
        // TODO: Extensions.Exceptions
#pragma warning disable CA2208, S3928 // Instantiate argument exceptions correctly
        var t =
            m.DeclaringTypeDefinition
            ?? throw new ArgumentNullException(nameof(m.DeclaringTypeDefinition));
#pragma warning restore CA2208, S3928 // Instantiate argument exceptions correctly

        if (t.Kind != TypeKind.Enum)
        {
            if (m.IsAbstract && t.Kind != TypeKind.Interface)
            {
                yield return ModelModifier.Abstract;
            }

            if (
                m.HasAttribute(KnownAttribute.AsyncStateMachine)
                || m.HasAttribute(KnownAttribute.AsyncIteratorStateMachine)
            )
            {
                yield return ModelModifier.Async;
            }

            if (m.HasAttribute(KnownAttribute.DllImport))
            {
                yield return ModelModifier.Extern;
            }

            if (m.IsOverride)
            {
                yield return ModelModifier.Override;
            }

            if (m.IsSealed)
            {
                yield return ModelModifier.Sealed;
            }

            if (m.IsStatic)
            {
                yield return ModelModifier.Static;
            }

            if (m.IsVirtual)
            {
                yield return ModelModifier.Virtual;
            }

            if (m is IMethod me)
            {
                foreach (var modifier in GetModifiers(me))
                {
                    yield return modifier;
                }
            }

            if (m is IField f)
            {
                foreach (var modifier in GetModifiers(f))
                {
                    yield return modifier;
                }
            }

            if (m is IProperty p)
            {
                foreach (var modifier in GetModifiers(p))
                {
                    yield return modifier;
                }
            }

            // TODO: unsafe
        }
    }

    private static IEnumerable<ModelModifier> GetModifiers(IField f)
    {
        if (f.IsConst)
        {
            yield return ModelModifier.Const;
        }

        if (f.IsReadOnly)
        {
            yield return ModelModifier.ReadOnly;
        }

        if (f.IsVolatile)
        {
            yield return ModelModifier.Volatile;
        }
    }

    private static IEnumerable<ModelModifier> GetModifiers(IMethod m)
    {
        if (m.ReturnTypeIsRefReadOnly)
        {
            yield return ModelModifier.Ref;
            yield return ModelModifier.ReadOnly;
        }
    }

    private static IEnumerable<ModelModifier> GetModifiers(IProperty p)
    {
        if (p.HasAttribute(KnownAttribute.RequiredAttribute))
        {
            yield return ModelModifier.Required;
        }
    }

    private static HashSet<ModelModifier> MapModifiers(ITypeDefinition typeDefinition) =>
        GetModifiers(typeDefinition).ToHashSet();

    private static HashSet<ModelModifier> MapModifiers(IMember member) =>
        GetModifiers(member).ToHashSet();

    private static bool HasFileAccessModifier(ITypeDefinition typeDefinition)
    {
        var entityName = typeDefinition.Name.Split(
            _fileTypeSeparator,
            StringSplitOptions.RemoveEmptyEntries
        );

        return entityName.Length > 1 && typeDefinition.Name.StartsWith('<');
    }

    private static bool TryGetDocFromBaseClass(
        IEntity entity,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result =
            entity is ITypeDefinition typeDefinition
            && typeDefinition
                .EnumerateBaseTypeDefinitions()
                .Any(type => TryGetDocumentation(type, resolver, out resultDocumentation));
        baseDocumentation = resultDocumentation;
        return result;
    }

    private static bool TryGetDocFromExplicit(
        IEntity entity,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result =
            entity is IMember member
            && member.IsExplicitInterfaceImplementation
            && member.ExplicitlyImplementedInterfaceMembers.Any(type =>
                TryGetDocumentation(type, resolver, out resultDocumentation)
            );
        baseDocumentation = resultDocumentation;
        return result;
    }

    private static bool TryGetDocFromInterface(
        IEntity entity,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result = false;
        if (entity is IMember member)
        {
            var parentName = member.DeclaringTypeDefinition!.FullName;
            var entityId = member.GetIdString();

            result =
                member.DeclaringTypeDefinition is not null
                && member
                    .DeclaringTypeDefinition.EnumerateBaseTypeDefinitions()
                    .Any(type =>
                    {
                        if (type.Kind != TypeKind.Interface)
                        {
                            return false;
                        }

                        _ = TryGetDocumentationProvider(type.ParentModule!, out _);

                        var lookupId = entityId.Replace(
                            parentName,
                            type.FullName
#if !NETSTANDARD2_0
                            ,
                            OrdinalIgnoreCase
#endif
                        );
                        resultDocumentation = GetDocumentation(lookupId, resolver);
                        return resultDocumentation is not null;
                    });
        }

        baseDocumentation = resultDocumentation;
        return result;
    }

    private static bool TryGetDocFromReference(
        string? reference,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        baseDocumentation = null;

        if (!string.IsNullOrEmpty(reference))
        {
            baseDocumentation = GetDocumentation(reference, resolver);
        }

        return baseDocumentation is not null;
    }

    internal static XElement? GetDocumentation(string? id, ITypeResolveContext resolver)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var entity = IdStringProvider.FindEntity(id, resolver);

        return TryGetDocumentation(entity, resolver, out var documentation) ? documentation : null;
    }

    internal static bool TryGetDocumentation(
        IEntity? entity,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? documentation
    )
    {
        documentation = null;

        if (entity is null || entity.ParentModule is null)
        {
            return false;
        }

        if (!TryGetDocumentationProvider(entity.ParentModule, out var documentationProvider))
        {
            return false;
        }

        var result = ConvertToDocumentation(documentationProvider.GetDocumentation(entity));

        if (result is null)
        {
            return false;
        }

        if (result.HasInheritDoc(out var inheritedDocumentation))
        {
            var reference = inheritedDocumentation.GetCRefAttribute();
            inheritedDocumentation.Remove();

            if (
                TryGetDocFromReference(reference, resolver, out var baseDocumentation)
                || TryGetDocFromExplicit(entity, resolver, out baseDocumentation)
                || TryGetDocFromBaseClass(entity, resolver, out baseDocumentation)
                || TryGetDocFromInterface(entity, resolver, out baseDocumentation)
            )
            {
                result = baseDocumentation.Merge(result, _ignoredElements);
            }
        }

        documentation = result;

        return documentation is not null;
    }

    private static XElement? ConvertToDocumentation(string? documentationString) =>
        string.IsNullOrWhiteSpace(documentationString)
            ? null
            : XElement.Parse($"<doc>{documentationString}</doc>");
}
