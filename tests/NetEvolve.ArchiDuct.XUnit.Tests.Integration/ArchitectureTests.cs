namespace NetEvolve.ArchiDuct.XUnit.Tests.Integration;

using Xunit;

public class ArchitectureTests(ArchitectureTestsProvider provider)
    : IClassFixture<ArchitectureTestsProvider>
{
    [Fact]
    public void Instance_AtLeastOneAssembly_Expected() =>
        Assert.NotEmpty(provider.Architecture.Assemblies);

    [Fact]
    public void Instance_AtLeastOneMember_Expected() =>
        Assert.NotEmpty(provider.Architecture.Members);

    [Fact]
    public void Instance_AtLeastOneNamespace_Expected() =>
        Assert.NotEmpty(provider.Architecture.Namespaces);

    [Fact]
    public void Instance_AtLeastOneType_Expected() => Assert.NotEmpty(provider.Architecture.Types);
}
