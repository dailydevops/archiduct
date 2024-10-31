namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleFileModifier_Tests(FileModifierTypeProvider provider)
    : TestCaseBase<FileModifierTypeProvider>(provider) { }

public sealed class FileModifierTypeProvider() : TypeProviderBase(typeof(FileModifier)) { }
#pragma warning disable CA1812
file sealed class FileModifier
{
    public int Counter { get; }
}
#pragma warning restore CA1812
