namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.ArgumentNullException>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemArgumentNullException_Tests(GenericTypeProvider<System.ArgumentNullException> provider)
    : TypesTestCaseGenericBase<System.ArgumentNullException>(provider) { }
