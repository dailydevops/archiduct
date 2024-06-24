namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemActionIntIntBool_Tests(
    GenericTypeProvider<System.Action<int, int, bool>> provider
) : TypesTestCaseGenericBase<System.Action<int, int, bool>>(provider, disableMembersCheck: true) { }
