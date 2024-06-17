namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using Xunit;

public abstract class TestCaseBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false
) : IClassFixture<GenericTypeProvider<TTestCase>>
    where TTestCase : notnull
{
    [Fact]
    public void Instance_AssemblyOne_Expected() => Assert.Single(provider.Architecture.Assemblies);

    [Fact]
    public void Instance_MembersNotEmpty_Expected()
    {
        if (disableMembersCheck)
        {
            Assert.True(true);
        }
        else
        {
            Assert.NotEmpty(provider.Architecture.Members);
        }
    }

    [Fact]
    public void Instance_NamespacesOne_Expected() =>
        Assert.Single(provider.Architecture.Namespaces);

    [Fact]
    public void Instance_TypesNotEmpty_Expected() => Assert.NotEmpty(provider.Architecture.Types);
}
