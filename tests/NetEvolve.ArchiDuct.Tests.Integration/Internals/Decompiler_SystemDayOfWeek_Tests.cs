﻿namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.DayOfWeek>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemDayOfWeek_Tests(GenericTypeProvider<System.DayOfWeek> provider)
    : TypesTestCaseGenericBase<System.DayOfWeek>(provider) { }
