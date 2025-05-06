namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Func<int, int, bool>>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemFuncIntIntBool_Tests(GenericTypeProvider<System.Func<int, int, bool>> provider)
    : TypesTestCaseGenericBase<System.Func<int, int, bool>>(provider, disableMembersCheck: true) { }
