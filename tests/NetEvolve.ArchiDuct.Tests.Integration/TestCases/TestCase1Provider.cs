namespace NetEvolve.ArchiDuct.Tests.Integration.TestCases;

using System;
using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Abstractions;
using NetEvolve.FluentValue;
using Xunit;

public sealed class TestCase1Provider : IAsyncLifetime
{
    public IArchitecture Architecture { get; private set; } = default!;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync() =>
        Architecture = await ArchitectureCollector
            .Create()
            .AddAssembly<TestCase2>()
            .FilterNamespace(
                Value.Not.Null.And.EqualTo(
                    "NetEvolve.ArchiDuct.Tests.Integration.TestCases",
                    StringComparison.Ordinal
                )
            )
            .CollectAsync()
            .ConfigureAwait(false);
}
