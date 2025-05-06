namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<ExtensionsTypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleExtensions_Tests(ExtensionsTypeProvider provider)
    : TestCaseBase<ExtensionsTypeProvider>(provider) { }

public class ExtensionsTypeProvider() : TypeProviderBase(typeof(ExampleExtensions)) { }

internal static class ExampleExtensions
{
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
}
