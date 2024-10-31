namespace NetEvolve.ArchiDuct.Extensions;

using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;

internal static class IMethodExtensions
{
    public static bool IsUnsafe(this IMethod method) =>
        (IsUnsafe(method.ReturnType) || method.Parameters.Any(p => IsUnsafe(p.Type)));

    private static bool IsUnsafe(IType type) =>
        type.Kind switch
        {
            TypeKind.Pointer or TypeKind.FunctionPointer => true,
            TypeKind.ByReference or TypeKind.Array when type is TypeWithElementType elementType =>
                IsUnsafe(elementType.ElementType),
            _ => false,
        };
}
