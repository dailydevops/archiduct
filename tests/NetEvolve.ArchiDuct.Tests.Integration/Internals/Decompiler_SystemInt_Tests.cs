namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<int>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemInt_Tests(GenericTypeProvider<int> provider) : TypesTestCaseGenericBase<int>(provider) { }
