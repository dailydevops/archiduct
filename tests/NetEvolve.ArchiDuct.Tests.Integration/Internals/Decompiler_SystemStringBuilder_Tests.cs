namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Text.StringBuilder>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemStringBuilder_Tests(GenericTypeProvider<System.Text.StringBuilder> provider)
    : TypesTestCaseGenericBase<System.Text.StringBuilder>(provider) { }
