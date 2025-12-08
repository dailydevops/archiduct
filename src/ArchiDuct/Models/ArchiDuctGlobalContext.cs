namespace ArchiDuct.Models;

public sealed partial class ArchiDuctGlobalContext
{
    private static readonly Lazy<ArchiDuctGlobalContext> _instance = new Lazy<ArchiDuctGlobalContext>(
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    public static ArchiDuctGlobalContext Instance => _instance.Value;

    internal bool IsGracefulStopRequested { get; set; }
}
