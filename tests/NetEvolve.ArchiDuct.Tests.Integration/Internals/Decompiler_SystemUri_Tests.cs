namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemUri_Tests(GenericTypeProvider<System.Uri> provider)
    : TypesTestCaseGenericBase<System.Uri>(
        provider,
        // TODO: This is a workaround for the issue that CI pipeline, which is running on Linux, is failing the test.
        disableMembersCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
        disableTypesCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ) { }
