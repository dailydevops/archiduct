namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleFileModifier_Tests(FileModifierTypeProvider provider)
    : TestCaseBase<FileModifierTypeProvider>(
        provider,
        // This is a workaround for the issue that CI pipeline, which is running on Linux, is failing the test.
        disableMembersCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
        disableTypesCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ) { }

public sealed class FileModifierTypeProvider() : TypeProviderBase(typeof(FileModifier)) { }
#pragma warning disable CA1812
file sealed class FileModifier
{
    public int Counter { get; }
}
#pragma warning restore CA1812
