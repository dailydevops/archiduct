namespace NetEvolve.ArchiDuct.Tests.Architecture.Legacy.Models;

using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class NamespaceTests
{
    private readonly IObjectProvider<IType> _types = Types()
        .That()
        .ResideInNamespace(@"^NetEvolve\.ArchiDuct\.Models.+", true);

    [Fact]
    public void Types_should_be_public()
    {
        var rule = Classes().That().Are(_types).Should().BePublic();

        rule.Check(ArchiDuctArchitecture.Instance);
    }

    [Fact]
    public void Constructors_should_be_restricted()
    {
        var rule = Members()
            .That()
            .AreDeclaredIn(_types)
            .And()
            .HaveNameContaining(".ctor")
            .Should()
            .BeInternal()
            .OrShould()
            .BePrivateProtected()
            .OrShould()
            .BePrivate();

        rule.Check(ArchiDuctArchitecture.Instance);
    }
}
