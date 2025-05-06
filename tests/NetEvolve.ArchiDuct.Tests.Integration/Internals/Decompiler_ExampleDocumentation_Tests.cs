namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.Runtime.InteropServices;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<DocumentationTypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleDocumentation_Tests(DocumentationTypeProvider provider)
    : TestCaseBase<DocumentationTypeProvider>(
        provider,
        enableDocumentationCheck: true,
        // Workaround for now, Linux is not detecting the documentation correct.
        operationSystems: [OSPlatform.Windows]
    ) { }

public sealed class DocumentationTypeProvider() : TypeProviderBase(typeof(DocumentatedClass)) { }

/// <summary>
/// This class is well documentated.
/// </summary>
/// <remarks>Hello World!</remarks>
public class DocumentatedClass
{
    /// <summary>
    /// Gets or sets the name of this class.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Validates the class.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the class is valid; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid() => Name != null;
}
