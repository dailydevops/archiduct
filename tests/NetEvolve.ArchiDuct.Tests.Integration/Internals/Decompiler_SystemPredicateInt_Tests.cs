namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemPredicateInt_Tests(
    GenericTypeProvider<System.Predicate<int>> provider
) : TypesTestCaseBase<System.Predicate<int>>(provider, disableMembersCheck: true) { }
