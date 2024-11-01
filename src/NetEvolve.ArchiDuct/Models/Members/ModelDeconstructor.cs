namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a deconstruct implementation.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/deconstruct"/>
public sealed class ModelDeconstructor : ModelMemberAdvancedBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Deconstructor;

    /// <inheritdoc />
    internal ModelDeconstructor(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) { }
}
