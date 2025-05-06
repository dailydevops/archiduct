namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<ArchitectureCollector>>(Shared = SharedType.PerClass)]
public class Decompiler_NetEvolveArchiDuct_Tests(GenericTypeProvider<ArchitectureCollector> provider)
    : AssembliesTestCaseBase<ArchitectureCollector>(provider) { }
