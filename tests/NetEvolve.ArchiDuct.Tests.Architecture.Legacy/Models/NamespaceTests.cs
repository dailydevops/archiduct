namespace NetEvolve.ArchiDuct.Tests.Architecture.Legacy.Models;

using ArchUnitNET.Domain;
using ArchUnitNET.TUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class NamespaceTests
{
    private readonly IObjectProvider<IType> _types = Types()
        .That()
        .ResideInNamespaceMatching(@"^NetEvolve\.ArchiDuct\.Models.+");

    [Test]
    public void Types_should_be_public()
    {
        var rule = Classes().That().Are(_types).Should().BePublic();

        rule.Check(ArchiDuctArchitecture.Instance);
    }

    [Test]
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
