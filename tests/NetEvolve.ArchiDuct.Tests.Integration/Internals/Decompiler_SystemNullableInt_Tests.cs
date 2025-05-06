namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<NullableIntProvider>(Shared = SharedType.PerClass)]
public class Decompiler_SystemNullableInt_Tests(NullableIntProvider provider)
    : TestCaseBase<NullableIntProvider>(provider) { }

public sealed class NullableIntProvider() : TypeProviderBase(typeof(int?)) { }
