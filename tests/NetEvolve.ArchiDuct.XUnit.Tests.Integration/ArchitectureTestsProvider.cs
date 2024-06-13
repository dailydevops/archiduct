namespace NetEvolve.ArchiDuct.XUnit.Tests.Integration;

using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using Xunit;

public sealed class ArchitectureTestsProvider : IAsyncLifetime
{
    public IArchitecture Architecture { get; private set; } = default!;

    public Task DisposeAsync() => Task.CompletedTask;

    public Task InitializeAsync()
    {
#pragma warning disable CA1849 // Call async methods when in an async method
        Architecture = ArchitectureCollector
            .Create()
            .AddAssembly<ArchitectureCollector>()
            .AddAssembly(typeof(XunitMarker))
            .Collect();
#pragma warning restore CA1849 // Call async methods when in an async method

        return Task.CompletedTask;
    }
}
