namespace ArchiDuct.Framework;

using System;
using System.Threading.Tasks;
using ArchiDuct.Logging;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using Microsoft.Testing.Platform.Logging;
using Microsoft.Testing.Platform.Services;

internal sealed class ArchiDuctTestFramework : ITestFramework
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly ITestFrameworkCapabilities _capabilities;
#pragma warning restore S4487 // Unread "private" fields should be removed
    private readonly ILogger _logger;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
    private readonly IServiceProvider _serviceProvider;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

    public string Uid => ArchiDuctFramework.Uid;

    public string Version => ArchiDuctFramework.Version;

    public string DisplayName => ArchiDuctFramework.DisplayName;

    public string Description => ArchiDuctFramework.Description;

    public ArchiDuctTestFramework(ITestFrameworkCapabilities capabilities, IServiceProvider serviceProvider)
    {
        _capabilities = capabilities;
        _serviceProvider = serviceProvider;

        var loggingFactory = _serviceProvider.GetService<ILoggerFactory>();
        _logger = loggingFactory?.CreateLogger<ArchiDuctTestFramework>() ?? NullLogger<ArchiDuctTestFramework>.Instance;
    }

    public async Task<CloseTestSessionResult> CloseTestSessionAsync(CloseTestSessionContext context)
    {
        await _logger.LogInformationAsync("Closing test session...").ConfigureAwait(false);
        return new CloseTestSessionResult { IsSuccess = false };
    }

    public Task<CreateTestSessionResult> CreateTestSessionAsync(CreateTestSessionContext context) =>
        Task.FromResult(new CreateTestSessionResult { IsSuccess = true });

    public Task ExecuteRequestAsync(ExecuteRequestContext context) => Task.CompletedTask;

    public Task<bool> IsEnabledAsync() => Task.FromResult(true);
}
