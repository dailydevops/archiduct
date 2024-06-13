namespace NetEvolve.ArchiDuct.Tests.Integration.TestCases;

using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using Xunit;

public sealed class GenericTestCaseProvider<TClass> : IAsyncLifetime
    where TClass : notnull
{
    public IArchitecture Architecture { get; private set; } = default!;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync() =>
        Architecture = await ArchitectureCollector
            .Create()
            .FilterType<TClass>()
            .CollectAsync()
            .ConfigureAwait(false);
}
