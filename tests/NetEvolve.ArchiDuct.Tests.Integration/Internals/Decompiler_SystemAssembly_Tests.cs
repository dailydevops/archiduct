namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<string>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemAssembly_Tests(GenericTypeProvider<string> provider)
    : AssembliesTestCaseBase<string>(provider) { }
