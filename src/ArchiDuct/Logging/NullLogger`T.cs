namespace ArchiDuct.Logging;

using System.Threading.Tasks;
using Microsoft.Testing.Platform.Logging;

/// <summary>
/// Provides a null implementation of <see cref="Microsoft.Testing.Platform.Logging.ILogger{TLogger}"/> that performs no logging operations.
/// </summary>
/// <typeparam name="TLogger">The type associated with the logger instance.</typeparam>
/// <remarks>
/// This class implements the Null Object pattern for logging, useful as a fallback when no actual logger is available.
/// All logging methods are no-ops, and <see cref="IsEnabled"/> always returns <see langword="false"/>.
/// </remarks>
internal sealed class NullLogger<TLogger> : ILogger<TLogger>
{
    private static readonly Lazy<NullLogger<TLogger>> _instance = new(() => new NullLogger<TLogger>());

    /// <summary>
    /// Gets the singleton instance of <see cref="NullLogger{TLogger}"/>.
    /// </summary>
    /// <value>
    /// The singleton instance of the null logger.
    /// </value>
    public static NullLogger<TLogger> Instance => _instance.Value;

    private NullLogger() { }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => false;

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) { }

    /// <inheritdoc />
    public Task LogAsync<TState>(
        LogLevel logLevel,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) => Task.CompletedTask;
}
