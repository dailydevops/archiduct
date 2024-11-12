namespace NetEvolve.ArchiDuct.Extensions;

using ICSharpCode.Decompiler.TypeSystem;

internal static class ITypeExtensions
{
    public static bool IsNullable(this IType type) =>
        type.Nullability == Nullability.Nullable || type.IsKnownType(KnownTypeCode.NullableOfT);
}
