namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.StringSplitOptions>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemStringSplitOptions_Tests(GenericTypeProvider<System.StringSplitOptions> provider)
    : TypesTestCaseGenericBase<System.StringSplitOptions>(provider) { }
