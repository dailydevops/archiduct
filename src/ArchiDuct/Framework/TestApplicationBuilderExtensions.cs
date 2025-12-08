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

    private static IReadOnlyCollection<ITestFrameworkCapability> GetCapabilities(IServiceProvider serviceProvider) =>
        [new BannerCapability(serviceProvider), new TrxReportCapability(), new GracefulStopTestExecutionCapability()];

    private static ITestApplicationBuilder RegisterCommandLineProvider(this ITestApplicationBuilder builder) =>
        // TODO: Implement command line provider for ArchiDuct if needed.
        builder;

    private static ITestApplicationBuilder RegisterTestHostProvider(this ITestApplicationBuilder builder) =>
        // TODO: Implement test host provider for ArchiDuct if needed.
        builder;
}
