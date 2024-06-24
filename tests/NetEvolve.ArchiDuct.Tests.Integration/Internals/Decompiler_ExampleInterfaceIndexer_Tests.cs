namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleInterfaceIndexer_Tests(InterfaceIndexerTypeProvider provider)
    : TestCaseBase<InterfaceIndexerTypeProvider>(provider) { }

public sealed class InterfaceIndexerTypeProvider() : TypeProviderBase(typeof(IIndexInterface)) { }

// Indexer on an interface:
public interface IIndexInterface
{
    // Indexer declaration:
    int this[int index] { get; set; }
}
