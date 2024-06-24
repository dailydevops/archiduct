namespace NetEvolve.ArchiDuct.Models.Members;

using System.Collections.Generic;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Internals;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Documentation;

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
    /// Describes the type of the parameter.
    /// </summary>
    public ModelReturn? Type { get; }

    /// <summary>
    /// Gets the optional value of this parameter. If IsOptional is false, this will be always null.
    /// </summary>
    public string? OptionalValue { get; }

    internal ModelParameter(IParameter parameter, ModelEntityBase parentEntity)
        : base(
            $"{parentEntity.Id}.{parameter.Name}",
            parameter.Name,
            $"{parentEntity.FullName}.{parameter.Name}",
            parentEntity,
            ModelDocumentation.LoadParameter(parentEntity.Documentation, parameter.Name)
        )
    {
        Modifiers = ModelFactory.MapModifiers(parameter);
        Type = ModelFactory.GetReturnType(parameter);

        if (parameter.IsOptional)
        {
            OptionalValue = parameter.GetConstantValue(true)?.ToString() ?? "<null>";
        }
    }
}
