namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

public abstract class TestCaseBase<TTypeProvider>(
    TTypeProvider provider,
    bool disableMembersCheck = false
) : IClassFixture<TTypeProvider>
    where TTypeProvider : notnull, TypeProviderBase
{
    private protected readonly TTypeProvider _provider = provider;

    protected bool DisableMembersCheck { get; } = disableMembersCheck;

    [Fact]
    public void Instance_AssemblyOne_Expected() => Assert.Single(_provider.Architecture.Assemblies);

    [Fact]
    public void Instance_MembersNotEmpty_Expected()
    {
        if (DisableMembersCheck)
        {
            Assert.True(true);
        }
        else
        {
            Assert.NotEmpty(_provider.Architecture.Members);
        }
    }

    [Fact]
    public void Instance_NamespacesOne_Expected() =>
        Assert.Single(_provider.Architecture.Namespaces);

    [Fact]
    public void Instance_TypesNotEmpty_Expected() => Assert.NotEmpty(_provider.Architecture.Types);

    [Fact]
    public async Task Verify_Members()
    {
        var members = _provider.Architecture.Members;

        if (DisableMembersCheck || members.Count == 0)
        {
            Assert.True(true);
        }
        else
        {
            _ = await Verifier.Verify(members);
        }
    }

    [Fact]
    public async Task Verify_Types() => _ = await Verifier.Verify(_provider.Architecture.Types);
}
