namespace NetEvolve.ArchiDuct.Extensions;

using System.Reflection.Metadata;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;

internal static class IMemberExtensions
{
    public static bool IsUnsafe(this IMember member) =>
        IsUnsafe(member.ReturnType)
        || (member is IParameterizedMember pm && pm.Parameters.Any(p => IsUnsafe(p.Type)))
        || (member is IMethod m && m.GetBody() is MethodBodyBlock body && body.IsUnsafe());

    private static bool IsUnsafe(IType type) =>
        type.Kind switch
        {
            TypeKind.Pointer or TypeKind.FunctionPointer => true,
            TypeKind.ByReference or TypeKind.Array when type is TypeWithElementType elementType =>
                IsUnsafe(elementType.ElementType),
            _ => false,
        };
}
