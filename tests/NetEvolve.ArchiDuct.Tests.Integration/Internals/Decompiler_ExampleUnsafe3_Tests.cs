namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleUnsafe3_Tests(UnsafeClass3TypeProvider provider)
    : TestCaseBase<UnsafeClass3TypeProvider>(provider) { }

public sealed class UnsafeClass3TypeProvider() : TypeProviderBase(typeof(UnsafeTest3)) { }

#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1051 // Do not declare visible instance fields
public unsafe struct UnsafeTest3
{
    public int Value;
    public UnsafeTest3* Left;
    public UnsafeTest3* Right;
}
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore CA1815 // Override equals and operator equals on value types
