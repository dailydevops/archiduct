namespace NetEvolve.ArchiDuct.Extensions;

using System.Runtime.CompilerServices;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;

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
