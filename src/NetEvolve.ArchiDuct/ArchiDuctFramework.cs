namespace NetEvolve.ArchiDuct;

/// <summary>
/// Provides constants that define metadata for the ArchiDuct integration of <see cref="Microsoft.Testing.Platform"/>.
/// </summary>
/// <remarks>
/// This class contains framework-level constants used throughout the ArchiDuct testing framework for identification,
/// versioning, and display purposes when integrating with the Microsoft Testing Platform.
/// </remarks>
internal static class ArchiDuctFramework
{
    /// <summary>
    /// Gets the display name of the ArchiDuct framework.
    /// </summary>
    /// <value>
    /// The string "ArchiDuct".
    /// </value>
    public const string DisplayName = "ArchiDuct";

    /// <summary>
    /// Gets the description of the ArchiDuct framework.
    /// </summary>
    /// <value>
    /// A description indicating that ArchiDuct is a lightweight architecture testing framework for .NET built on <see cref="Microsoft.Testing.Platform"/>.
    /// </value>
    public const string Description =
        "ArchiDuct is a lightweight architecture testing framework for .NET built directly on Microsoft.Testing.Platform.";

    /// <summary>
    /// Gets the unique identifier (UID) of the ArchiDuct framework.
    /// </summary>
    /// <value>
    /// The unique identifier string "ArchiDuct.Framework".
    /// </value>
    public const string Uid = "ArchiDuct.Framework";

    /// <summary>
    /// Gets the version number of the <see cref="ArchiDuctFramework"/> assembly as a string.
    /// </summary>
    /// <value>
    /// The assembly version in three-part format (major.minor.build), or "1.0.0" if the version cannot be determined.
    /// </value>
    public static string Version { get; } =
        typeof(ArchiDuctFramework).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
}
