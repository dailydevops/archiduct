﻿namespace NetEvolve.ArchiDuct.Internals;

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
using NetEvolve.Arguments;
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
        ModelTypeBase parent,
        XElement? doc,
        ITypeResolveContext resolver
    )
    {
        ModelMemberBase model = member switch
        {
            IField field when member.DeclaringType.Kind == TypeKind.Enum => new ModelEnumMember(
                field,
                parent,
                doc
            ),
            IField field => new ModelField(field, parent, doc),
            IProperty property when property.IsExplicitInterfaceImplementation =>
                new ModelExplicitProperty(property, parent, doc),
            IProperty property when property.IsIndexer => new ModelIndexer(property, parent, doc),
            IProperty property => new ModelProperty(property, parent, doc),
            IMethod method when method.IsExplicitInterfaceImplementation => new ModelExplicitMethod(
                method,
                parent,
                doc
            ),
            IMethod method when method.IsConstructor && method.IsStatic =>
                new ModelStaticConstructor(method, parent, doc),
            IMethod method when method.IsConstructor => new ModelConstructor(method, parent, doc),
            IMethod method when method.IsOperator => new ModelOperator(method, parent, doc),
            IMethod method when method.IsDestructor => new ModelDestructor(method, parent, doc),
            IMethod method when method.Name.Equals("Deconstruct", Ordinal) =>
                new ModelDeconstructor(method, parent, doc),
            IMethod method when method.IsExtensionMethod => new ModelExtensionMethod(
                method,
                parent,
                doc
            ),
            IMethod method => new ModelMethod(method, parent, doc),
            IEvent @event when @event.IsExplicitInterfaceImplementation => new ModelExplicitEvent(
                @event,
                parent,
                doc
            ),
            IEvent @event => new ModelEvent(@event, parent, doc),
            _ => throw new InvalidOperationException(),
        };
        model.Modifiers = MapModifiers(member);
        model.Attributes = MapAttributes(member, resolver);

        return model;
    }

    public static ModelTypeBase CreateModelType(
        ITypeDefinition typeDefinition,
        ModelEntityBase parent,
        XElement? doc,
        ITypeResolveContext resolver
    )
    {
#pragma warning disable IDE0072 // Add missing cases
        ModelTypeBase model = typeDefinition.Kind switch
        {
            TypeKind.Class => new ModelClass(typeDefinition, parent, doc),
            TypeKind.Interface => new ModelInterface(typeDefinition, parent, doc),
            TypeKind.Struct => new ModelStruct(typeDefinition, parent, doc),
            TypeKind.Delegate => new ModelDelegate(typeDefinition, parent, doc),
            TypeKind.Enum => new ModelEnum(typeDefinition, parent, doc),
            _ => throw new InvalidOperationException(),
        };
#pragma warning restore IDE0072 // Add missing cases

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
            returnId =
                member.MemberDefinition is IMethod method
                && method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal))
                    ? $"T:{method.ReflectionName}.{returnType.Name}"
                    : $"T:{member.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
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
            returnId = method.TypeParameters.Any(x => x.Name.Equals(returnType.Name, Ordinal))
                ? $"T:{method.ReflectionName}.{returnType.Name}"
                : $"T:{method.DeclaringTypeDefinition!.ReflectionName}.{returnType.Name}";
        }

        return new ModelReturn(returnId, returnType.Nullability == Nullability.Nullable);
    }

    internal static HashSet<ModelModifier> MapModifiers(IParameter parameter)
    {
        var modifiers = new HashSet<ModelModifier>();
        if (parameter.IsIn)
        {
            _ = modifiers.Add(ModelModifier.In);
        }

        if (parameter.IsRef)
        {
            var requiresLocation = parameter
                .GetAttributes()
                .Any(x => x.AttributeType.Name == "RequiresLocationAttribute");
            if (requiresLocation)
            {
                _ = modifiers.Add(ModelModifier.ReadOnly);
            }
            _ = modifiers.Add(ModelModifier.Ref);
        }

        if (parameter.IsOut)
        {
            _ = modifiers.Add(ModelModifier.Out);
        }

        if (parameter.IsParams)
        {
            _ = modifiers.Add(ModelModifier.Params);
        }

        if (
            parameter.Owner is IMethod method
            && (method.IsExtensionMethod || method.HasAttribute(KnownAttribute.Extension))
            && method.Parameters[0] == parameter
        )
        {
            _ = modifiers.Add(ModelModifier.This);
        }

        return modifiers;
    }

    private static HashSet<ModelModifier> MapModifiers(ITypeDefinition typeDefinition)
    {
        if (typeDefinition.Kind == TypeKind.Enum)
        {
            return [];
        }

        var modifiers = new HashSet<ModelModifier>();
        if (typeDefinition.IsAbstract && typeDefinition.Kind != TypeKind.Interface)
        {
            _ = modifiers.Add(ModelModifier.Abstract);
        }

        if (typeDefinition.IsReadOnly)
        {
            _ = modifiers.Add(ModelModifier.ReadOnly);
        }

        if (typeDefinition.IsSealed && typeDefinition.Kind != TypeKind.Struct)
        {
            _ = modifiers.Add(ModelModifier.Sealed);
        }

        if (typeDefinition.IsStatic)
        {
            _ = modifiers.Add(ModelModifier.Static);
        }

        if (typeDefinition.IsByRefLike && typeDefinition.Kind == TypeKind.Struct)
        {
            _ = modifiers.Add(ModelModifier.Ref);
        }

        return modifiers;
    }

    private static HashSet<ModelModifier> MapModifiers(IMember member)
    {
        Argument.ThrowIfNull(member.DeclaringTypeDefinition);
        var t = member.DeclaringTypeDefinition;

        if (t.Kind == TypeKind.Enum)
        {
            return [];
        }

        var modifiers = new HashSet<ModelModifier>();
        if (member.IsAbstract && t.Kind != TypeKind.Interface)
        {
            _ = modifiers.Add(ModelModifier.Abstract);
        }

        if (
            member.HasAttribute(KnownAttribute.AsyncStateMachine)
            || member.HasAttribute(KnownAttribute.AsyncIteratorStateMachine)
        )
        {
            _ = modifiers.Add(ModelModifier.Async);
        }

        if (member.IsOverride)
        {
            _ = modifiers.Add(ModelModifier.Override);
        }

        if (member.IsSealed)
        {
            _ = modifiers.Add(ModelModifier.Sealed);
        }

        if (member.IsStatic)
        {
            _ = modifiers.Add(ModelModifier.Static);
        }

        if (member.IsVirtual)
        {
            _ = modifiers.Add(ModelModifier.Virtual);
        }

        if (member.IsUnsafe())
        {
            _ = modifiers.Add(ModelModifier.Unsafe);
        }

        if (member is IMethod method)
        {
            if (method.IsExtern())
            {
                _ = modifiers.Add(ModelModifier.Extern);
            }

            if (
                method
                    .GetAttributes()
                    .Any(x =>
                        x.AttributeType.Name.Equals("LibraryImportAttribute", OrdinalIgnoreCase)
                    )
            )
            {
                _ = modifiers.Add(ModelModifier.Partial);
            }

            if (method.ReturnTypeIsRefReadOnly)
            {
                _ = modifiers.Add(ModelModifier.Ref);
                _ = modifiers.Add(ModelModifier.ReadOnly);
            }
        }

        if (member is IField field)
        {
            if (field.IsConst)
            {
                _ = modifiers.Add(ModelModifier.Const);
            }

            if (field.IsReadOnly)
            {
                _ = modifiers.Add(ModelModifier.ReadOnly);
            }

            if (field.IsVolatile)
            {
                _ = modifiers.Add(ModelModifier.Volatile);
            }
        }

        if (member is IProperty property)
        {
            if (property.HasAttribute(KnownAttribute.RequiredAttribute))
            {
                _ = modifiers.Add(ModelModifier.Required);
            }
        }

        return modifiers;
    }

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
            var parent = member.DeclaringTypeDefinition!.FullName;
            var entityId = member.GetIdString();
            var typeDefinition = member.DeclaringTypeDefinition;

            result =
                typeDefinition is not null
                && typeDefinition
                    .EnumerateBaseTypeDefinitions()
                    .Any(type =>
                    {
                        if (type.Kind != TypeKind.Interface)
                        {
                            return false;
                        }

                        _ = TryGetDocumentationProvider(type.ParentModule!, out _);

                        var lookupId = entityId.Replace(parent, type.FullName, OrdinalIgnoreCase);
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

        return TryGetDocumentation(entity, resolver, out var doc) ? doc : null;
    }

    internal static bool TryGetDocumentation(
        IEntity? entity,
        ITypeResolveContext resolver,
        [NotNullWhen(true)] out XElement? doc
    )
    {
        doc = null;

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

        doc = result;

        return doc is not null;
    }

    private static XElement? ConvertToDocumentation(string? documentationString) =>
        string.IsNullOrWhiteSpace(documentationString)
            ? null
            : XElement.Parse($"<doc>{documentationString}</doc>");
}
