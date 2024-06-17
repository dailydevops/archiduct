namespace NetEvolve.ArchiDuct.Models.Members;

using System.Collections.Generic;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Internals;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <inheritdoc />
public sealed class ModelParameter : ModelEntityBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Parameter;

    /// <summary>
    /// Collection of all modifiers for this type.
    /// </summary>
    public HashSet<ModelModifier> Modifiers { get; }

    /// <summary>
    /// Returns the type id of the parameter.
    /// </summary>
    public string TypeId { get; }

    /// <summary>
    /// Gets the optional value of this parameter. If IsOptional is false, this will be always null.
    /// </summary>
    public string? OptionalValue { get; }

    /// <inheritdoc />
    public override string? Remarks => null;

    /// <inheritdoc />
    public override string? Returns => null;

    /// <inheritdoc />
    public override string? Summary => this.GetParameterDocumentation(Name);

    internal ModelParameter(IParameter parameter, ModelEntityBase parentEntity)
        : base(
            $"{parentEntity.Id}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            parentEntity._documentation
        )
    {
        Modifiers = ModelFactory.MapModifiers(parameter);
        TypeId = ModelFactory.GetReturnTypeId(parameter);

        if (parameter.IsOptional)
        {
            OptionalValue = parameter.GetConstantValue(true)?.ToString() ?? "<null>";
        }
    }
}
