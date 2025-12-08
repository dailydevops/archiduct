namespace ArchiDuct.Tests.Unit;

using System;
using System.Threading.Tasks;
using ArchiDuct;
using TUnit.Assertions;

public sealed class ArchiDuctFrameworkTests
{
    [Test]
    public async Task Framework_constants_match_expected_values()
    {
        _ = await Assert.That(string.IsNullOrWhiteSpace(ArchiDuctFramework.DisplayName)).IsFalse();
        _ = await Assert.That(string.IsNullOrWhiteSpace(ArchiDuctFramework.Uid)).IsFalse();
        _ = await Assert.That(string.IsNullOrWhiteSpace(ArchiDuctFramework.Version)).IsFalse();
        _ = await Assert
            .That(
                ArchiDuctFramework.Description.Contains(
                    "lightweight architecture testing framework",
                    StringComparison.Ordinal
                )
            )
            .IsTrue();
    }
}
