namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

public abstract class TestCaseGenericBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck
) : TestCaseBase<GenericTypeProvider<TTestCase>>(provider, disableMembersCheck)
    where TTestCase : notnull { }
