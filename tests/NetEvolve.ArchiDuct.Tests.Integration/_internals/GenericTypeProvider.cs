namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using Xunit;

public sealed class GenericTypeProvider<TType> : IAsyncLifetime
    where TType : notnull
{
    public IArchitecture Architecture { get; private set; } = default!;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync() =>
        Architecture = await ArchitectureCollector
            .Create()
            .FilterType<TType>()
            .CollectAsync()
            .ConfigureAwait(false);
}
