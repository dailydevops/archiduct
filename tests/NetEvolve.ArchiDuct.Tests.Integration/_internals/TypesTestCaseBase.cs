namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

public abstract class TypesTestCaseBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false
) : TestCaseBase<TTestCase>(provider, disableMembersCheck)
    where TTestCase : notnull
{
    [Fact]
    public async Task Verify_Types() => _ = await Verifier.Verify(_provider.Architecture.Types);
}
