namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemFuncIntIntBool_Tests(GenericTypeProvider<System.Func<int, int, bool>> provider)
    : TypesTestCaseGenericBase<System.Func<int, int, bool>>(provider, disableMembersCheck: true) { }
