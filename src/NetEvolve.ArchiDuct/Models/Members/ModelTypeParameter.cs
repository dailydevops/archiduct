namespace NetEvolve.ArchiDuct.Models.Members;

using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Represents a type parameter description.
/// </summary>
public sealed class ModelTypeParameter : ModelEntityBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.TypeParameter;

    /// <inheritdoc />
    public override string? Remarks => null;

    /// <inheritdoc />
    public override string? Returns => null;

    /// <inheritdoc />
    public override string? Summary => this.GetTypeParameterDocumentation(Name);

    /// <inheritdoc />
    internal ModelTypeParameter(ITypeParameter parameter, ModelEntityBase parentEntity)
        : base(
            $"{parentEntity.Id.Replace("M:", "G:", OrdinalIgnoreCase)}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            parentEntity._documentation
        ) { }
}
