namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

public abstract class TestCaseGenericBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false,
    bool disableTypesCheck = false
) : TestCaseBase<GenericTypeProvider<TTestCase>>(provider, disableMembersCheck, disableTypesCheck)
    where TTestCase : notnull { }
