namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;

[InheritsTests]
public abstract class AssembliesTestCaseBase<TTestCase>(GenericTypeProvider<TTestCase> provider)
    : TestCaseBase<GenericTypeProvider<TTestCase>>(provider, disableMembersCheck: true)
    where TTestCase : notnull
{
    [Test]
    [SkipCI]
    public async Task Verify_Architecture()
    {
        var architecture = _provider.Architecture;
        _ = await Verify(architecture).IgnoreParameters();
    }

    [Test]
    [SkipCI]
    public async Task Verify_Assemblies()
    {
        var architecture = _provider.Architecture;

        _ = Parallel.ForEach(
            architecture.Assemblies,
            assembly =>
            {
                assembly.Value.Types.Clear();
                assembly.Value.Members.Clear();
            }
        );
        _ = await Verify(architecture.Assemblies).IgnoreParameters();
    }
}
