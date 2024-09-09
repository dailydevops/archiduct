namespace NetEvolve.ArchiDuct.Tests.Architecture.Legacy;

using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class NamespaceTests
{
    private readonly IObjectProvider<IType> _types = Types()
        .That()
        .ResideInNamespace("NetEvolve.ArchiDuct");

    [Fact]
    public void Types_should_be_public()
    {
        var rule = Classes().That().Are(_types).Should().BePublic();

        rule.Check(ArchiDuctArchitecture.Instance);
    }

    [Fact]
    public void Types_should_be_sealed()
    {
        var rule = Classes().That().Are(_types).Should().BeSealed();

        rule.Check(ArchiDuctArchitecture.Instance);
    }
}
