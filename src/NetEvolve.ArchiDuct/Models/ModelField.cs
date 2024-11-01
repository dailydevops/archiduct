namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a field implementation.
/// </summary>
public sealed class ModelField : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Field;

    /// <inheritdoc />
    internal ModelField(IField field, ModelTypeBase parent, XElement? doc)
        : base(field, parent, doc) { }
}
