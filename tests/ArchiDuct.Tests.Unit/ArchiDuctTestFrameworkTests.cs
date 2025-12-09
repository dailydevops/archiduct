namespace ArchiDuct.Tests.Unit;

using System;
using System.Threading.Tasks;
using ArchiDuct;
using ArchiDuct.Framework;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using TUnit.Assertions;

public sealed class ArchiDuctTestFrameworkTests
{
    [Test]
    public async Task Metadata_uses_framework_constants()
    {
        var framework = CreateFramework();

        _ = await Assert.That(framework.DisplayName).IsEqualTo(ArchiDuctFramework.DisplayName);
        _ = await Assert.That(framework.Description).IsEqualTo(ArchiDuctFramework.Description);
        _ = await Assert.That(framework.Uid).IsEqualTo(ArchiDuctFramework.Uid);
        _ = await Assert.That(framework.Version).IsEqualTo(ArchiDuctFramework.Version);
    }

    [Test]
    public async Task Create_session_returns_success()
    {
        var result = await CreateFramework().CreateTestSessionAsync(null!);

        _ = await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Close_session_returns_failure()
    {
        var result = await CreateFramework().CloseTestSessionAsync(null!);

        _ = await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task Execute_request_completes() => await CreateFramework().ExecuteRequestAsync(null!);

    [Test]
    public async Task Is_enabled_returns_true() =>
        _ = await Assert.That(await CreateFramework().IsEnabledAsync()).IsTrue();

    private static ArchiDuctTestFramework CreateFramework() =>
        new(new TestFrameworkCapabilities(Array.Empty<ITestFrameworkCapability>()), new TestServiceProvider());
}
