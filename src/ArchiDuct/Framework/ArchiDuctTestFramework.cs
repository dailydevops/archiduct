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
    private readonly ITestFrameworkCapabilities _capabilities;
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

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

    public Task<CloseTestSessionResult> CloseTestSessionAsync(CloseTestSessionContext context) =>
        Task.FromResult(new CloseTestSessionResult { IsSuccess = false });

    public Task<CreateTestSessionResult> CreateTestSessionAsync(CreateTestSessionContext context) =>
        Task.FromResult(new CreateTestSessionResult { IsSuccess = true });

    public Task ExecuteRequestAsync(ExecuteRequestContext context) => Task.CompletedTask;

    public Task<bool> IsEnabledAsync() => Task.FromResult(true);
}
