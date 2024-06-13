namespace NetEvolve.ArchiDuct.Models.Members;

using System.Collections.Generic;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Types;

/// <inheritdoc />
public sealed class ModelParameter : ModelEntityBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Parameter;

    /// <summary>
    /// Collection of all modifiers for this type.
    /// </summary>
    public IEnumerable<ModelModifier> Modifiers { get; }

    /// <inheritdoc />
    public override string? Remarks => null;

    /// <inheritdoc />
    public override string? Returns => null;

    /// <inheritdoc />
    public override string? Summary => this.GetParameterDocumentation(Name);

    /// <inheritdoc />
    internal ModelParameter(IParameter parameter, ModelMemberAdvancedBase parentEntity)
        : base(
            $"{parentEntity.Id}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            parentEntity._documentation
        ) => Modifiers = ModelFactory.MapModifiers(parameter);

    /// <inheritdoc />
    internal ModelParameter(IParameter parameter, ModelDelegate parentEntity)
        : base(
            $"{parentEntity.Id}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            parentEntity._documentation
        ) => Modifiers = ModelFactory.MapModifiers(parameter);

    /// <inheritdoc />
    internal ModelParameter(IParameter parameter, ModelIndexer parentEntity)
        : base(
            $"{parentEntity.Id}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            parentEntity._documentation
        ) => Modifiers = ModelFactory.MapModifiers(parameter);
}
