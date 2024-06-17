namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemIComparable_Tests(GenericTypeProvider<System.IComparable> provider)
    : TestCaseBase<System.IComparable>(provider, disableMembersCheck: true) { }
