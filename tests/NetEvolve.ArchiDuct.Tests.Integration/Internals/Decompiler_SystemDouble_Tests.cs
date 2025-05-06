namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<double>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemDouble_Tests(GenericTypeProvider<double> provider)
    : TypesTestCaseGenericBase<double>(provider) { }
