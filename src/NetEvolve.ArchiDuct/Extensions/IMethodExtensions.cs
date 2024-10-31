namespace NetEvolve.ArchiDuct.Extensions;

using System.Runtime.CompilerServices;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;

internal static class IMemberExtensions
{
    public static bool IsUnsafe(this IMember member) =>
        IsUnsafe(member.ReturnType)
        || (member is IParameterizedMember method && method.Parameters.Any(p => IsUnsafe(p.Type)));

    private static bool IsUnsafe(IType type) =>
        type.Kind switch
        {
            TypeKind.Pointer or TypeKind.FunctionPointer => true,
            TypeKind.ByReference or TypeKind.Array when type is TypeWithElementType elementType =>
                IsUnsafe(elementType.ElementType),
            _ => false,
        };
}

internal static class IMethodExtensions
{
    public static bool IsExtern(this IMethod method)
    {
        if (method.HasBody || method.IsAbstract)
        {
            return false;
        }

        if (method.HasAttribute(KnownAttribute.DllImport))
        {
            return true;
        }

        if (
            method.GetAttribute(KnownAttribute.MethodImpl) is IAttribute attr
            && attr.FixedArguments.Length == 1
            && attr.FixedArguments[0].Value is object value
            && value.Equals((int)MethodImplOptions.InternalCall)
        )
        {
            return true;
        }

        return false;
    }
}
