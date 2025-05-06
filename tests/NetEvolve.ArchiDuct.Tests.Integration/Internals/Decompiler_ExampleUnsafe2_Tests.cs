namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<UnsafeClass2TypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleUnsafe2_Tests(UnsafeClass2TypeProvider provider)
    : TestCaseBase<UnsafeClass2TypeProvider>(provider) { }

public sealed class UnsafeClass2TypeProvider() : TypeProviderBase(typeof(UnsafeTest2)) { }

internal static unsafe class UnsafeTest2
{
    internal static void SquarePtrParam(int* p) => *p *= *p;

    public static void Test()
    {
        var i = 5;
        // Unsafe method: uses address-of operator (&).
        SquarePtrParam(&i);
        Console.WriteLine(i);
    }
}
