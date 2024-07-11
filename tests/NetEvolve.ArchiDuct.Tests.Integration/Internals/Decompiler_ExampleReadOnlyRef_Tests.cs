namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleReadOnlyRef_Tests(ConversionRequestTypeProvider provider)
    : TestCaseBase<ConversionRequestTypeProvider>(provider) { }

public sealed class ConversionRequestTypeProvider()
    : TypeProviderBase(typeof(ConversionRequest)) { }

public readonly ref struct ConversionRequest
{
    public ConversionRequest(double rate, ReadOnlySpan<double> values)
    {
        Rate = rate;
        Values = values;
    }

    public double Rate { get; }
    public ReadOnlySpan<double> Values { get; }
}
