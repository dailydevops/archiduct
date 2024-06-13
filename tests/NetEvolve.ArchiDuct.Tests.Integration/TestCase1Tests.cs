namespace NetEvolve.ArchiDuct.Tests.Integration;

using NetEvolve.ArchiDuct.Tests.Integration.TestCases;
using Xunit;

public class TestCase1Tests(TestCase1Provider provider) : IClassFixture<TestCase1Provider>
{
    [Fact]
    public void Instance_OneAssembly_Expected() => Assert.Single(provider.Architecture.Assemblies);

    [Fact]
    public void Instance_OneType_Expected() => Assert.NotEmpty(provider.Architecture.Types);
}
