namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<Half>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemHalf_Tests(GenericTypeProvider<Half> provider)
    : TypesTestCaseGenericBase<Half>(provider) { }
