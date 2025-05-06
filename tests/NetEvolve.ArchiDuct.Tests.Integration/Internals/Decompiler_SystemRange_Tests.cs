namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Range>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemRange_Tests(GenericTypeProvider<System.Range> provider)
    : TypesTestCaseGenericBase<System.Range>(provider) { }
