﻿namespace NetEvolve.ArchiDuct.Extensions;

using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;

internal static class IEntityExtensions
{
    public static bool HasDerivedImplementations(this IEntity entity) =>
        entity is IMember member && (member.IsOverride || member.IsExplicitInterfaceImplementation);

    public static bool IsDefaultConstructor(this IEntity entity) =>
        entity is IMethod method
        && method.IsConstructor
        && !method.Parameters.Any()
        && (
            method.DeclaringType.Kind is TypeKind.Struct or TypeKind.Enum
            || (
                method.DeclaringTypeDefinition is ITypeDefinition declaringType
                && declaringType.GetConstructors().Count() == 1
            )
        );

    public static string GetQualifiedName(this IEntity entity) =>
        $"{entity.ParentModule?.AssemblyName}, Version={entity.ParentModule?.AssemblyVersion}, {entity.GetIdString()}";
}
