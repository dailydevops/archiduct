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
        DocumentationXmlPropertyConstants.Summary
    ];

    private static readonly ConcurrentDictionary<
        IModule,
        IDocumentationProvider
    > _documentationProviders = new ConcurrentDictionary<IModule, IDocumentationProvider>(
        new ModuleEqualityComparer()
    );

    public static bool TryGetDocumentationProvider(
        IModule module,
        [NotNullWhen(true)] out IDocumentationProvider? documentationProvider
    )
    {
        if (module?.PEFile is null)
        {
            documentationProvider = null;
            return false;
        }

        documentationProvider = _documentationProviders.GetOrAdd(
            module,
            m =>
            {
                return XmlDocLoader.LoadDocumentation(m.PEFile)
                    ?? XmlDocLoader.MscorlibDocumentation;
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
            IField enumMember when member.DeclaringType.Kind == TypeKind.Enum
                => new ModelEnumMember(enumMember, parentModel, documentation),
            IField constant when constant.IsConst
                => new ModelConstant(constant, parentModel, documentation),
            IField field => new ModelField(field, parentModel, documentation),
            IProperty explicitProperty when explicitProperty.IsExplicitInterfaceImplementation
                => new ModelExplicitProperty(explicitProperty, parentModel, documentation),
            IProperty indexer when indexer.IsIndexer
                => new ModelIndexer(indexer, parentModel, documentation),
            IProperty property => new ModelProperty(property, parentModel, documentation),
            IMethod explicitMethod when explicitMethod.IsExplicitInterfaceImplementation
                => new ModelExplicitMethod(explicitMethod, parentModel, documentation),
            IMethod constructor when constructor.IsConstructor && constructor.IsStatic
                => new ModelStaticConstructor(constructor, parentModel, documentation),
            IMethod constructor when constructor.IsConstructor
                => new ModelConstructor(constructor, parentModel, documentation),
            IMethod @operator when @operator.IsOperator
                => new ModelOperator(@operator, parentModel, documentation),
            IMethod destructor when destructor.IsDestructor
                => new ModelDestructor(destructor, parentModel, documentation),
            IMethod deconstruct when deconstruct.Name.Equals("Deconstruct", Ordinal)
                => new ModelDeconstructor(deconstruct, parentModel, documentation),
            IMethod extensionMethod when extensionMethod.IsExtensionMethod
                => new ModelExtensionMethod(extensionMethod, parentModel, documentation),
            IMethod method => new ModelMethod(method, parentModel, documentation),
            IEvent @event when @event.IsExplicitInterfaceImplementation
                => new ModelExplicitEvent(@event, parentModel, documentation),
            IEvent @event => new ModelEvent(@event, parentModel, documentation),
            _ => throw new InvalidOperationException()
        };

        model.ReturnTypeId = GetReturnTypeId(member);
        model.Modifiers = MapModifiers(member);
        model.Attributes = MapAttributes(member, resolver);

        return model;
    }

    public static ModelTypeBase CreateModelType(
        ITypeDefinition typeDefinition,
        ModelEntityBase parentEntity,
        XElement? documentation,
        ITypeResolveContext resolver
    )
    {
#pragma warning disable IDE0072 // Add missing cases
        ModelTypeBase model = typeDefinition.Kind switch
        {
            TypeKind.Class when typeDefinition.IsRecord
                => new ModelRecordClass(typeDefinition, parentEntity, documentation),
            TypeKind.Class when !typeDefinition.IsRecord
                => new ModelClass(typeDefinition, parentEntity, documentation),
            TypeKind.Interface => new ModelInterface(typeDefinition, parentEntity, documentation),
            TypeKind.Struct when typeDefinition.IsRecord
                => new ModelRecordStruct(typeDefinition, parentEntity, documentation),
            TypeKind.Struct when !typeDefinition.IsRecord
                => new ModelStruct(typeDefinition, parentEntity, documentation),
            TypeKind.Delegate => new ModelDelegate(typeDefinition, parentEntity, documentation),
            TypeKind.Enum => new ModelEnum(typeDefinition, parentEntity, documentation),
            _ => throw new InvalidOperationException(),
        };
#pragma warning restore IDE0072 // Add missing cases

        model.BaseTypes = typeDefinition.GetAllBaseTypeIds();
        model.Modifiers = MapModifiers(typeDefinition);
        model.Implementations = typeDefinition.GetAllImplementationIds();
        model.InheritedMembers = typeDefinition.GetAllInheritedMemberIds();
        model.NamespaceId = parentEntity.Id;
        model.Attributes = MapAttributes(typeDefinition, resolver);

        return model;
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

                    _ = set.Add(
                        new ModelAttribute(
                            attribute,
                            typeDefinition,
                            GetDocumentation(typeDefinition.GetIdString(), resolver)
                        )
                    );
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

                    _ = set.Add(
                        new ModelAttribute(
                            attribute,
                            attributeTypeDefinition,
                            GetDocumentation(typeDefinition.GetIdString(), resolver)
                        )
                    );
                    return set;
                }
            );

    internal static string GetReturnTypeId(IMember member)
    {
        var returnType = member.ReturnType;
        if (returnType.Kind == TypeKind.TypeParameter)
        {
            if (
                member.MemberDefinition is IMethod method
                && method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal))
            )
            {
                return $"T:{method.ReflectionName}.{returnType.Name}";
            }

            return $"T:{member.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
        }

        return $"T:{returnType.ReflectionName}";
    }

    internal static string GetReturnTypeId(IParameter parameter)
    {
        var returnType = parameter.Type;
        if (returnType.Kind == TypeKind.TypeParameter && parameter.Owner is IMethod method)
        {
            if (method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal)))
            {
                return $"T:{method.ReflectionName}.{returnType.Name}";
            }

            return $"T:{method.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
        }

        return $"T:{returnType.ReflectionName}";
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
        if (HasFileAccessModifier(type))
        {
            yield return ModelModifier.File;
        }

        if (type.Accessibility == Public)
        {
            yield return ModelModifier.Public;
        }

        if (type.Accessibility is Protected or ProtectedAndInternal or ProtectedOrInternal)
        {
            yield return ModelModifier.Protected;
        }

        if (type.Accessibility is Internal or ProtectedOrInternal)
        {
            yield return ModelModifier.Internal;
        }

        if (type.Accessibility is Private or ProtectedAndInternal)
        {
            yield return ModelModifier.Private;
        }

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

        if (t.Kind != TypeKind.Enum && !m.IsExplicitInterfaceImplementation)
        {
            if (m.Accessibility == Public)
            {
                yield return ModelModifier.Public;
            }

            if (m.Accessibility is Protected or ProtectedAndInternal or ProtectedOrInternal)
            {
                yield return ModelModifier.Protected;
            }

            if (m.Accessibility is Internal or ProtectedOrInternal)
            {
                yield return ModelModifier.Internal;
            }

            if (m.Accessibility is Private or ProtectedAndInternal)
            {
                yield return ModelModifier.Private;
            }
        }

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

        if (p.ReturnTypeIsRefReadOnly)
        {
            yield return ModelModifier.Ref;
            yield return ModelModifier.ReadOnly;
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

        return entityName.Length > 1
            && typeDefinition.Name.StartsWith($"<{entityName[1]}>", Ordinal);
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

        if (reference is not null)
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
