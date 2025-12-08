namespace ArchiDuct.Models;

/// <summary>
/// Represents the global context for the ArchiDuct testing framework, providing thread-safe singleton access
/// to shared state across test execution lifecycle.
/// </summary>
/// <remarks>
/// This class uses the singleton pattern with <see cref="System.Lazy{T}"/> to ensure thread-safe initialization.
/// It maintains global state that is shared across the entire test execution process.
/// </remarks>
public sealed partial class ArchiDuctGlobalContext
{
    private static readonly Lazy<ArchiDuctGlobalContext> _instance = new Lazy<ArchiDuctGlobalContext>(
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    /// <summary>
    /// Gets the singleton instance of <see cref="ArchiDuctGlobalContext"/>.
    /// </summary>
    /// <value>
    /// The singleton instance of the global context.
    /// </value>
    public static ArchiDuctGlobalContext Instance => _instance.Value;

    /// <summary>
    /// Gets or sets a value indicating whether a graceful stop has been requested for test execution.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if a graceful stop is requested; otherwise, <see langword="false"/>.
    /// </value>
    internal bool IsGracefulStopRequested { get; set; }
}
