namespace ArchiDuct.Framework.Capabilities;

using System;
using System.Threading.Tasks;
using ArchiDuct;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using Microsoft.Testing.Platform.Services;

/// <summary>
/// Implements the banner message capability for ArchiDuct, displaying framework and platform information at test execution start.
/// </summary>
/// <remarks>
/// This capability provides a formatted banner message that includes the ArchiDuct framework version and the underlying platform information.
/// </remarks>
internal sealed class BannerCapability : IBannerMessageOwnerCapability
{
    private readonly IServiceProvider _serviceProvider;
    private const string Separator = "----------------------------------------";

    /// <summary>
    /// Initializes a new instance of the <see cref="BannerCapability"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving platform information implementing <see cref="System.IServiceProvider"/>.</param>
    public BannerCapability(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public Task<string?> GetBannerMessageAsync()
    {
        var platformInformation = _serviceProvider.GetRequiredService<IPlatformInformation>();

        return Task.FromResult<string?>(
            $"""
            {ArchiDuctFramework.DisplayName} v{ArchiDuctFramework.Version} running on {platformInformation.Name} v{platformInformation.Version}
            {Separator}
            """
        );
    }
}
