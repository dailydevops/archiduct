namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<GenericTypeProvider<System.Text.Json.Serialization.JsonConverter>>(Shared = SharedType.PerClass)]
public class Decompiler_SystemTextJsonSerializationJsonConverter_Tests(
    GenericTypeProvider<System.Text.Json.Serialization.JsonConverter> provider
) : TypesTestCaseGenericBase<System.Text.Json.Serialization.JsonConverter>(provider) { }
