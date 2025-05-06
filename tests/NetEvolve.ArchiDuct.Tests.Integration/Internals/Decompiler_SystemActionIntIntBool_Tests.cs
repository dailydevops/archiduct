namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Action<int, int, bool>>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemActionIntIntBool_Tests(GenericTypeProvider<System.Action<int, int, bool>> provider)
    : TypesTestCaseGenericBase<System.Action<int, int, bool>>(provider, disableMembersCheck: true) { }
