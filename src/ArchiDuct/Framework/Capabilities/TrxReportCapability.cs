namespace ArchiDuct.Framework.Capabilities;

using Microsoft.Testing.Extensions.TrxReport.Abstractions;

/// <summary>
/// Capability to indicate that ArchiDuct supports TRX reporting.
/// </summary>
internal sealed class TrxReportCapability : ITrxReportCapability
{
    /// <inheritdoc cref="ITrxReportCapability.IsSupported"/>
    public bool IsSupported { get; } = true;

    /// <inheritdoc cref="ITrxReportCapability.Enable"/>
    public void Enable() { }
}
