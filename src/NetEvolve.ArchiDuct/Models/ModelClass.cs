namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a class implementation.
/// </summary>
public sealed class ModelClass : ModelTypeBase
{
    /// <summary>
    /// Gets a value indicating whether the class is a <see langword="record"/> definition.
    /// </summary>
    public bool IsRecord { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Class;

    /// <inheritdoc />
    internal ModelClass(ITypeDefinition typeDefinition, ModelBase parent, XElement? doc)
        : base(typeDefinition, parent, doc) => IsRecord = typeDefinition.IsRecord;
}
