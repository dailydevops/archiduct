namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using ICSharpCode.Decompiler.TypeSystem;
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
    /// Describes the type of the parameter.
    /// </summary>
    public ModelType? Type { get; }

    /// <summary>
    /// Gets the optional value of this parameter. If IsOptional is false, this will be always null.
    /// </summary>
    public string? OptionalValue { get; }

    internal ModelParameter(IParameter parameter, ModelEntityBase parent)
        : base(
            $"{parent.Id}.{parameter.Name}",
            parameter.Name,
            $"{parent.FullName}.{parameter.Name}",
            parent,
            ModelDocumentation.LoadParameter(parent.Documentation, parameter.Name)
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
