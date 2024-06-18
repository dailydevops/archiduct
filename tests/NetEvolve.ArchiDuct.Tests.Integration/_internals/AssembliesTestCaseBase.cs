namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

public abstract class AssembliesTestCaseBase<TTestCase>(GenericTypeProvider<TTestCase> provider)
    : TestCaseBase<TTestCase>(provider, true)
    where TTestCase : notnull
{
    [Fact]
    public async Task Verify_Assemblies()
    {
        _ = Parallel.ForEach(
            _provider.Architecture.Assemblies,
            assembly => assembly.Value.Types.Clear()
        );
        _ = await Verifier.Verify(_provider.Architecture.Assemblies);
    }
}
