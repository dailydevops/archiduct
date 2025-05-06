namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.IDisposable>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemIDisposable_Tests(GenericTypeProvider<System.IDisposable> provider)
    : TypesTestCaseGenericBase<System.IDisposable>(provider) { }
