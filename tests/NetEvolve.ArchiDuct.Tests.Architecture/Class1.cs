namespace NetEvolve.ArchiDuct.Tests.Architecture;

using NetEvolve.ArchiDuct.Abstractions;

public class NamespaceTests
{
    [ScopeDefinition]
    public IScopeDefinition<ScopeNamespace> Scope => null!;

    [ScopeUsage]
    public ValueTask Namespaces(ScopeNamespace scope) => ValueTask.CompletedTask;
}

#if NETFRAMEWORK
internal static class ValueTaskExtensions
{
    extension(ValueTask)
    {
        public static ValueTask CompletedTask => default;
    }
}
#endif

public sealed class ScopeNamespace : IScope<ScopeNamespace> { }
