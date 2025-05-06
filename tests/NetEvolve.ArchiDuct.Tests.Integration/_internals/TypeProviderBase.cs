namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using TUnit.Core.Interfaces;

public abstract class TypeProviderBase(Type type) : IAsyncInitializer
{
    public IArchitecture Architecture { get; private set; } = default!;

    public async Task InitializeAsync() =>
        Architecture = await ArchitectureCollector.Create().FilterType(type).CollectAsync().ConfigureAwait(false);
}
