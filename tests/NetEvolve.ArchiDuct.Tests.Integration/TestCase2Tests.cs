namespace NetEvolve.ArchiDuct.Tests.Integration;

using NetEvolve.ArchiDuct.Tests.Integration.TestCases;
using Xunit;

public class TestCase2Tests(GenericTestCaseProvider<TestCase2> provider)
    : IClassFixture<GenericTestCaseProvider<TestCase2>>
{
    [Fact]
    public void Instance_OneAssembly_Expected() => Assert.Single(provider.Architecture.Assemblies);

    [Fact]
    public void Instance_OneType_Expected() => Assert.Single(provider.Architecture.Types);
}
