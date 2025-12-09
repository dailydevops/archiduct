namespace ArchiDuct.Framework;

using ArchiDuct.Framework.Capabilities;
using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using NetEvolve.Arguments;

/// <summary>
/// Provides extension methods for configuring ArchiDuct test framework support in an <see cref="ITestApplicationBuilder"/> instance.
/// </summary>
/// <remarks>These extension methods enable integration of ArchiDuct-specific capabilities and providers into the
/// test application builder. Use these methods to register the ArchiDuct test framework when setting up test
/// environments that require ArchiDuct features.</remarks>
internal static class TestApplicationBuilderExtensions
{
    /// <summary>
    /// Registers the ArchiDuct test framework with the test application builder.
    /// </summary>
    /// <param name="builder">The test application builder instance implementing <see cref="Microsoft.Testing.Platform.Builder.ITestApplicationBuilder"/>.</param>
    /// <returns>The test application builder for method chaining.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method configures the ArchiDuct test framework by registering its capabilities, test framework implementation,
    /// command line provider, and test host provider with the testing platform.
    /// </remarks>
    public static ITestApplicationBuilder AddArchiDuct(this ITestApplicationBuilder builder)
    {
        Argument.ThrowIfNull(builder);

        return builder
            .RegisterTestFramework(
                serviceProvider => new TestFrameworkCapabilities(GetCapabilities(serviceProvider)),
                (capabilities, serviceProvider) => new ArchiDuctTestFramework(capabilities, serviceProvider)
            )
            .RegisterCommandLineProvider()
            .RegisterTestHostProvider();
    }

    /// <summary>
    /// Gets the collection of test framework capabilities supported by ArchiDuct.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution implementing <see cref="System.IServiceProvider"/>.</param>
    /// <returns>A read-only collection of <see cref="Microsoft.Testing.Platform.Capabilities.TestFramework.ITestFrameworkCapability"/> instances.</returns>
    private static IReadOnlyCollection<ITestFrameworkCapability> GetCapabilities(IServiceProvider serviceProvider) =>
        [new BannerCapability(serviceProvider), new TrxReportCapability(), new GracefulStopTestExecutionCapability()];

    /// <summary>
    /// Registers the command line provider for ArchiDuct test framework.
    /// </summary>
    /// <param name="builder">The test application builder instance implementing <see cref="Microsoft.Testing.Platform.Builder.ITestApplicationBuilder"/>.</param>
    /// <returns>The test application builder for method chaining.</returns>
    /// <remarks>
    /// This is a placeholder for future command line provider implementation.
    /// </remarks>
    private static ITestApplicationBuilder RegisterCommandLineProvider(this ITestApplicationBuilder builder) =>
        // Implement command line provider for ArchiDuct if needed.
        builder;

    /// <summary>
    /// Registers the test host provider for ArchiDuct test framework.
    /// </summary>
    /// <param name="builder">The test application builder instance implementing <see cref="Microsoft.Testing.Platform.Builder.ITestApplicationBuilder"/>.</param>
    /// <returns>The test application builder for method chaining.</returns>
    /// <remarks>
    /// This is a placeholder for future test host provider implementation.
    /// </remarks>
    private static ITestApplicationBuilder RegisterTestHostProvider(this ITestApplicationBuilder builder) =>
        // Implement test host provider for ArchiDuct if needed.
        builder;
}
