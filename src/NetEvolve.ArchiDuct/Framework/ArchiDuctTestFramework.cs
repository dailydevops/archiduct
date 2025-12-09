namespace ArchiDuct.Framework;

using System;
using System.Threading.Tasks;
using ArchiDuct.Logging;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using Microsoft.Testing.Platform.Logging;
using Microsoft.Testing.Platform.Services;

/// <summary>
/// Implements the core test framework adapter for ArchiDuct, integrating with <see cref="Microsoft.Testing.Platform.Extensions.TestFramework.ITestFramework"/>.
/// </summary>
/// <remarks>
/// This class provides the primary integration point between ArchiDuct and the Microsoft Testing Platform,
/// managing test session lifecycle and execution contexts.
/// </remarks>
internal sealed class ArchiDuctTestFramework : ITestFramework
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly ITestFrameworkCapabilities _capabilities;
#pragma warning restore S4487 // Unread "private" fields should be removed
    private readonly ILogger _logger;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
    private readonly IServiceProvider _serviceProvider;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

    /// <inheritdoc />
    public string Uid => ArchiDuctFramework.Uid;

    /// <inheritdoc />
    public string Version => ArchiDuctFramework.Version;

    /// <inheritdoc />
    public string DisplayName => ArchiDuctFramework.DisplayName;

    /// <inheritdoc />
    public string Description => ArchiDuctFramework.Description;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArchiDuctTestFramework"/> class.
    /// </summary>
    /// <param name="capabilities">The test framework capabilities provided by <see cref="Microsoft.Testing.Platform.Capabilities.TestFramework.ITestFrameworkCapabilities"/>.</param>
    /// <param name="serviceProvider">The service provider for dependency resolution implementing <see cref="System.IServiceProvider"/>.</param>
    public ArchiDuctTestFramework(ITestFrameworkCapabilities capabilities, IServiceProvider serviceProvider)
    {
        _capabilities = capabilities;
        _serviceProvider = serviceProvider;

        var loggingFactory = _serviceProvider.GetService<ILoggerFactory>();
        _logger = loggingFactory?.CreateLogger<ArchiDuctTestFramework>() ?? NullLogger<ArchiDuctTestFramework>.Instance;
    }

    /// <inheritdoc />
    public async Task<CloseTestSessionResult> CloseTestSessionAsync(CloseTestSessionContext context)
    {
        await _logger.LogInformationAsync("Closing test session...").ConfigureAwait(false);
        return new CloseTestSessionResult { IsSuccess = true };
    }

    /// <inheritdoc />
    public Task<CreateTestSessionResult> CreateTestSessionAsync(CreateTestSessionContext context) =>
        Task.FromResult(new CreateTestSessionResult { IsSuccess = true });

    /// <inheritdoc />
    public Task ExecuteRequestAsync(ExecuteRequestContext context)
    {
        context.Complete();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<bool> IsEnabledAsync() => Task.FromResult(true);
}
