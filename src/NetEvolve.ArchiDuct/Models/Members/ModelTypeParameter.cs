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
    internal ModelTypeParameter(ITypeParameter parameter, ModelEntityBase parentEntity)
        : base(
            $"{parentEntity.Id.Replace("M:", "G:", OrdinalIgnoreCase)}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            ModelDocumentation.LoadTypeParameter(parentEntity.Documentation, parameter.Name)
        ) { }
}
