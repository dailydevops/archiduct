namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemUri_Tests(GenericTypeProvider<System.Uri> provider)
    : TypesTestCaseGenericBase<System.Uri>(
        provider,
        disableMembersCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
        disableTypesCheck: !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ) { }
