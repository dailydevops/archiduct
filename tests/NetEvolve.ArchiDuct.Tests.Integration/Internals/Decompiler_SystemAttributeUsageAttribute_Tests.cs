namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.AttributeUsageAttribute>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemAttributeUsageAttribute_Tests(
    GenericTypeProvider<System.AttributeUsageAttribute> provider
) : TypesTestCaseGenericBase<System.AttributeUsageAttribute>(provider) { }
