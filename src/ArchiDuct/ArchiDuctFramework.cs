namespace ArchiDuct;

/// <summary>
/// Provides constants that define metadata for the ArchiDuct integration of <b>Microsoft.Testing.Platform</b>.
/// </summary>
internal static class ArchiDuctFramework
{
    /// <summary>
    /// Gets the display name of the ArchiDuct framework.
    /// </summary>
    public const string DisplayName = "ArchiDuct";

    /// <summary>
    /// Gets the description of the ArchiDuct framework.
    /// </summary>
    public const string Description =
        "ArchiDuct is a lightweight architecture testing framework for .NET built directly on Microsoft.Testing.Platform.";

    /// <summary>
    /// Gets the unique identifier (UID) of the ArchiDuct framework.
    /// </summary>
    public const string Uid = "ArchiDuct.Framework";

    /// <summary>
    /// Gets the version number of the ArchiDuctFramework assembly as a string.
    /// </summary>
    public static string Version { get; } =
        typeof(ArchiDuctFramework).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
}
