namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<char>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemChar_Tests(GenericTypeProvider<char> provider)
    : TypesTestCaseGenericBase<char>(provider) { }
