namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleClassIndexer_Tests(ClassIndexerTypeProvider provider)
    : TestCaseBase<ClassIndexerTypeProvider>(provider) { }

public sealed class ClassIndexerTypeProvider() : TypeProviderBase(typeof(IndexerClass)) { }

public class IndexerClass : IIndexInterface
{
    private readonly int[] arr = new int[100];
    public int this[int index] // indexer declaration
    {
        // The arr object will throw IndexOutOfRange exception.
        get => arr[index];
        set => arr[index] = value;
    }
}
