namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleFinalizer_Tests(FinalizerClassTypeProvider provider)
    : TestCaseBase<FinalizerClassTypeProvider>(provider) { }

public sealed class FinalizerClassTypeProvider() : TypeProviderBase(typeof(FinalizerClass)) { }

public class FinalizerClass
{
    public override string ToString() => GetType().Name;

    ~FinalizerClass() => Console.WriteLine($"The {ToString()} finalizer is executing.");
}
