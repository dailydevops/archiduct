namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemNullableInt_Tests(NullableIntProvider provider)
    : TestCaseBase<NullableIntProvider>(provider) { }

public sealed class NullableIntProvider() : TypeProviderBase(typeof(int?)) { }
