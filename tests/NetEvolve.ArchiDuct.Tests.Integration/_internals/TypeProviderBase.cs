namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using Xunit;

public abstract class TypeProviderBase(Type type) : IAsyncLifetime
{
    public IArchitecture Architecture { get; private set; } = default!;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync() =>
        Architecture = await ArchitectureCollector
            .Create()
            .FilterType(type)
            .CollectAsync()
            .ConfigureAwait(false);
}
