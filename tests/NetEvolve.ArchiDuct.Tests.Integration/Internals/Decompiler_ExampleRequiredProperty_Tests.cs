namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<ExampleRequiredProperty>>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleRequiredProperty_Tests(GenericTypeProvider<ExampleRequiredProperty> provider)
    : TypesTestCaseGenericBase<ExampleRequiredProperty>(provider) { }

public class ExampleRequiredProperty
{
    public required string Name { get; set; } = string.Empty;
}
