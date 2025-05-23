﻿namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
public abstract class TypesTestCaseGenericBase<TTestCase>(
    GenericTypeProvider<TTestCase> provider,
    bool disableMembersCheck = false,
    bool disableTypesCheck = false
) : TestCaseGenericBase<TTestCase>(provider, disableMembersCheck, disableTypesCheck)
    where TTestCase : notnull { }
