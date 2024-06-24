namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

public abstract class TypesTestCaseGenericBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false
) : TestCaseGenericBase<TTestCase>(provider, disableMembersCheck)
    where TTestCase : notnull { }
