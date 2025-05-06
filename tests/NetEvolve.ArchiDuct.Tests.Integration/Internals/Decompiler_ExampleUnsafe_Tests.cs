namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<UnsafeClassTypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleUnsafe_Tests(UnsafeClassTypeProvider provider)
    : TestCaseBase<UnsafeClassTypeProvider>(provider) { }

public sealed class UnsafeClassTypeProvider() : TypeProviderBase(typeof(UnsafeTest)) { }

internal static class UnsafeTest
{
    internal static unsafe void SquarePtrParam(int* p) => *p *= *p;

    public static unsafe void Test()
    {
        var i = 5;
        // Unsafe method: uses address-of operator (&).
        SquarePtrParam(&i);
        Console.WriteLine(i);
    }
}
