namespace ArchiDuct.Framework.Capabilities;

using System.Threading;
using System.Threading.Tasks;
using ArchiDuct.Models;
using Microsoft.Testing.Platform.Capabilities.TestFramework;

internal sealed class GracefulStopTestExecutionCapability : IGracefulStopTestExecutionCapability
{
    public Task StopTestExecutionAsync(CancellationToken cancellationToken)
    {
        ArchiDuctGlobalContext.Instance.IsGracefulStopRequested = true;
        return Task.CompletedTask;
    }
}
