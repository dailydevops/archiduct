namespace NetEvolve.ArchiDuct.Internals;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
    internal static readonly string[] _fileTypeSeparator = ["__"];

    public static ModelMemberBase CreateModelMemberType(
        IMember member,
        ModelTypeBase parentModel,
        XElement? documentation
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

        return model;
    }

    public static ModelTypeBase CreateModelType(
        ITypeDefinition typeDefinition,
        ModelEntityBase parentEntity,
        XElement? documentation
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

        return model;
    }

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
}
