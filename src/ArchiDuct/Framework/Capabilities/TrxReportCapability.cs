namespace ArchiDuct.Framework.Capabilities;

using Microsoft.Testing.Extensions.TrxReport.Abstractions;

/// <summary>
/// Implements the TRX report capability for ArchiDuct, enabling generation of Visual Studio Test Results (TRX) files.
/// </summary>
/// <remarks>
/// This capability allows ArchiDuct to integrate with the TRX reporting extension from <see cref="Microsoft.Testing.Extensions.TrxReport.Abstractions"/>,
/// enabling test results to be exported in the standardized TRX format for consumption by Visual Studio and Azure DevOps.
/// </remarks>
internal sealed class TrxReportCapability : ITrxReportCapability
{
    /// <inheritdoc />
    public bool IsSupported { get; } = true;

    /// <inheritdoc />
    public void Enable() { }
}
