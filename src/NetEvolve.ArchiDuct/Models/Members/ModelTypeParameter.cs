namespace NetEvolve.ArchiDuct.Models.Members;

using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Documentation;

/// <summary>
/// Represents a type parameter description.
/// </summary>
public sealed class ModelTypeParameter : ModelEntityBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.TypeParameter;

    /// <inheritdoc />
    internal ModelTypeParameter(ITypeParameter parameter, ModelEntityBase parent)
        : base(
            $"{parent.Id.Replace("M:", "G:", OrdinalIgnoreCase)}.{parameter.Name}",
            parameter.Name,
            $"{parent.FullName}.{parameter.Name}",
            parent,
            ModelDocumentation.LoadTypeParameter(parent.Documentation, parameter.Name)
        ) { }
}
