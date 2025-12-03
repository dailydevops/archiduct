namespace NetEvolve.ArchiDuct.Tests.Architecture.Legacy;

using ArchUnitNET.Domain;
using ArchUnitNET.TUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class NamespaceTests
{
    private readonly IObjectProvider<IType> _types = Types().That().ResideInNamespace("NetEvolve.ArchiDuct");

    [Test]
    public void Types_should_be_public()
    {
        var rule = Classes().That().Are(_types).Should().BePublic();

        rule.Check(ArchiDuctArchitecture.Instance);
    }

    [Test]
    public void Types_should_be_sealed()
    {
        var rule = Classes().That().Are(_types).And().AreNotAbstract().Should().BeSealed();

        rule.Check(ArchiDuctArchitecture.Instance);
    }
}
