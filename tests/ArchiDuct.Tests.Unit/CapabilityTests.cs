namespace ArchiDuct.Tests.Unit;

using System.Threading.Tasks;
using ArchiDuct.Framework.Capabilities;
using TUnit.Assertions;

public sealed class CapabilityTests
{
    [Test]
    public async Task Trx_report_capability_is_supported()
    {
        var capability = new TrxReportCapability();

        _ = await Assert.That(capability.IsSupported).IsTrue();
        capability.Enable();
    }
}
