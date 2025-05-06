namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.ArgumentException>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemArgumentException_Tests(GenericTypeProvider<System.ArgumentException> provider)
    : TypesTestCaseGenericBase<System.ArgumentException>(provider) { }
