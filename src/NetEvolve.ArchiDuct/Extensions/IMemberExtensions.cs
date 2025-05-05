namespace NetEvolve.ArchiDuct.Extensions;

using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;

internal static class IMemberExtensions
{
    public static bool IsReturnTypeIsRefReadOnly(this IMember member) =>
        (member is IMethod me && me.ReturnTypeIsRefReadOnly)
        || (member is IProperty pe && pe.ReturnTypeIsRefReadOnly)
        || (member is IField fe && fe.ReturnTypeIsRefReadOnly);

    public static bool IsUnsafe(this IMember member) =>
        IsUnsafe(member.ReturnType)
        || (member is IParameterizedMember method && method.Parameters.Any(p => IsUnsafe(p.Type)));

#pragma warning disable IDE0072 // Add missing cases
    private static bool IsUnsafe(IType type) =>
        type.Kind switch
        {
            TypeKind.Pointer or TypeKind.FunctionPointer => true,
            TypeKind.ByReference or TypeKind.Array when type is TypeWithElementType elementType => IsUnsafe(
                elementType.ElementType
            ),
            _ => false,
        };
#pragma warning restore IDE0072 // Add missing cases
}
