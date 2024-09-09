namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using System.Reflection;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleExtensionMethod_Tests(ExtensionExampleTypeProvider provider)
    : TestCaseBase<ExtensionExampleTypeProvider>(provider) { }

public sealed class ExtensionExampleTypeProvider() : TypeProviderBase(typeof(ExtensionExample)) { }

public static class ExtensionExample
{
    public static void Deconstruct(
        this PropertyInfo p,
        out bool isStatic,
        out bool isReadOnly,
        out bool isIndexed,
        out Type propertyType
    )
    {
        ArgumentNullException.ThrowIfNull(p);

        var getter = p.GetMethod;
        isReadOnly = !p.CanWrite;
        isStatic = getter!.IsStatic;
        isIndexed = p.GetIndexParameters().Length > 0;
        propertyType = p.PropertyType;
    }

    public static void Deconstruct<T>(this T? nullable, out bool hasValue, out T value)
        where T : struct
    {
        hasValue = nullable.HasValue;
        value = nullable.GetValueOrDefault();
    }
}
