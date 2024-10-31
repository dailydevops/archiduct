namespace NetEvolve.ArchiDuct.Models.Types;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Internals;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Members;

/// <summary>
/// Describes a delegate implementation.
/// </summary>
[SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "As designed."
)]
public sealed class ModelDelegate : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Delegate;

    /// <inheritdoc />
    public HashSet<ModelParameter> Parameters { get; } = default!;

    /// <summary>
    /// Gets the return type.
    /// </summary>
    public ModelReturn Type { get; } = default!;

    /// <inheritdoc />
    internal ModelDelegate(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation)
    {
        if (typeDefinition.GetDelegateInvokeMethod() is IMethod delegateMethod)
        {
            Parameters = delegateMethod
                .Parameters.Select(p => new ModelParameter(p, this))
                .ToHashSet();

            Type = ModelFactory.GetReturnType(
                delegateMethod,
                delegateMethod.ReturnTypeIsRefReadOnly
            )!;
        }
    }
}
