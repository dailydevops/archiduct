namespace ArchiDuct.Logging;

using System.Threading.Tasks;
using Microsoft.Testing.Platform.Logging;

internal sealed class NullLogger<TLogger> : ILogger<TLogger>
{
    private static readonly Lazy<NullLogger<TLogger>> _instance = new(() => new NullLogger<TLogger>());

    public static NullLogger<TLogger> Instance => _instance.Value;

    private NullLogger() { }

    public bool IsEnabled(LogLevel logLevel) => false;

    public void Log<TState>(
        LogLevel logLevel,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) { }

    public Task LogAsync<TState>(
        LogLevel logLevel,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) => Task.CompletedTask;
}
