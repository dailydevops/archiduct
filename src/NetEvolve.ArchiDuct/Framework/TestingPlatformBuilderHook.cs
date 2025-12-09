namespace NetEvolve.ArchiDuct.Framework;

using Microsoft.Testing.Platform.Builder;

/// <summary>
/// Provides extension point for <c>Microsoft.Testing.Platform.MSBuild</c> to automatically integrate ArchiDuct into the testing platform builder.
/// </summary>
/// <remarks>
/// This class is automatically discovered and invoked by the MSBuild integration of <see cref="Microsoft.Testing.Platform"/>
/// to register ArchiDuct framework extensions during test application initialization.
/// </remarks>
public static class TestingPlatformBuilderHook
{
    /// <summary>
    /// Adds ArchiDuct framework extensions to the testing platform builder.
    /// </summary>
    /// <param name="testApplicationBuilder">The test application builder instance implementing <see cref="ITestApplicationBuilder"/>.</param>
    /// <param name="args">The command line args passed to the test application (unused by this implementation).</param>
    /// <remarks>
    /// This method is called automatically by the MSBuild testing platform integration to register the ArchiDuct test framework.
    /// </remarks>
    public static void AddExtensions(ITestApplicationBuilder testApplicationBuilder, string[] args) =>
        testApplicationBuilder.AddArchiDuct();
}
