namespace NetEvolve.ArchiDuct.Framework.Capabilities;

using System.Threading;
using System.Threading.Tasks;
using ArchiDuct.Models;
using Microsoft.Testing.Platform.Capabilities.TestFramework;

/// <summary>
/// Implements the graceful stop capability for ArchiDuct test execution, allowing tests to be stopped cleanly without abrupt termination.
/// </summary>
/// <remarks>
/// This capability sets a flag in the <see cref="ArchiDuctGlobalContext"/> to signal that test execution should stop gracefully,
/// allowing in-progress tests to complete before terminating the test session.
/// </remarks>
internal sealed class GracefulStopTestExecutionCapability : IGracefulStopTestExecutionCapability
{
    /// <inheritdoc />
    public Task StopTestExecutionAsync(CancellationToken cancellationToken)
    {
        ArchiDuctGlobalContext.Instance.IsGracefulStopRequested = true;
        return Task.CompletedTask;
    }
}
