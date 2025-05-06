namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<DocumentationTypeProvider2>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleDocumentation2_Tests(DocumentationTypeProvider2 provider)
    : TestCaseBase<DocumentationTypeProvider2>(
        provider,
        enableDocumentationCheck: true,
        // Workaround for now, Linux is not detecting the documentation correct.
        operationSystems: [OSPlatform.Windows]
    ) { }

public sealed class DocumentationTypeProvider2() : TypeProviderBase(typeof(DocumentatedClass2)) { }

public class DocumentatedClass2 : IEqualityComparer<DocumentatedClass2>
{
    /// <inheritdoc />
    public override bool Equals(object? obj) => true;

    /// <inheritdoc />
    public bool Equals(DocumentatedClass2? x, DocumentatedClass2? y) => true;

    public override int GetHashCode() => 0;

    /// <inheritdoc />
    int IEqualityComparer<DocumentatedClass2>.GetHashCode([DisallowNull] DocumentatedClass2 obj) => 0;
}
