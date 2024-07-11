#if NET7_0_OR_GREATER
namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleRequiredProperty_Tests(
    GenericTypeProvider<ExampleRequiredProperty> provider
) : TypesTestCaseGenericBase<ExampleRequiredProperty>(provider) { }

public class ExampleRequiredProperty
{
    public required string Name { get; set; } = string.Empty;
}
#endif
