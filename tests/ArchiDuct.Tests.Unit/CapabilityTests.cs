namespace ArchiDuct.Tests.Unit;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArchiDuct.Framework.Capabilities;
using ArchiDuct.Models;
using Microsoft.Testing.Platform.Services;
using TUnit.Assertions;

public sealed class CapabilityTests
{
    [Test]
    public async Task Banner_capability_includes_platform_information()
    {
        var capability = new BannerCapability(
            new TestServiceProvider(
                new Dictionary<Type, object>
                {
                    [typeof(IPlatformInformation)] = new PlatformInformationStub("TestPlatform", "1.2.3"),
                }
            )
        );

        var banner = await capability.GetBannerMessageAsync();

        _ = await Assert.That(banner).IsNotNull();
        _ = await Assert.That(banner).Contains("ArchiDuct v", StringComparison.Ordinal);
        _ = await Assert.That(banner).Contains("TestPlatform v1.2.3", StringComparison.Ordinal);
    }

    [Test]
    public async Task Graceful_stop_sets_global_flag()
    {
        ArchiDuctGlobalContext.Instance.IsGracefulStopRequested = false;
        var capability = new GracefulStopTestExecutionCapability();

        await capability.StopTestExecutionAsync(CancellationToken.None);

        _ = await Assert.That(ArchiDuctGlobalContext.Instance.IsGracefulStopRequested).IsTrue();
        ArchiDuctGlobalContext.Instance.IsGracefulStopRequested = false;
    }

    [Test]
    public async Task Trx_report_capability_is_supported()
    {
        var capability = new TrxReportCapability();

        _ = await Assert.That(capability.IsSupported).IsTrue();
        capability.Enable();
    }
}
