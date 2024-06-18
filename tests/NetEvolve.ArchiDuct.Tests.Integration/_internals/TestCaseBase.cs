namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using Xunit;

public abstract class TestCaseBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false
) : IClassFixture<GenericTypeProvider<TTestCase>>
    where TTestCase : notnull
{
    private protected readonly GenericTypeProvider<TTestCase> _provider = provider;

    [Fact]
    public void Instance_AssemblyOne_Expected() => Assert.Single(_provider.Architecture.Assemblies);

    [Fact]
    public void Instance_MembersNotEmpty_Expected()
    {
        if (disableMembersCheck)
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
}
