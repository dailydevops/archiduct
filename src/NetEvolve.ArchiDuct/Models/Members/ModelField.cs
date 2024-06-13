namespace NetEvolve.ArchiDuct.Models.Members;

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
    internal ModelField(IField field, ModelTypeBase parentEntity, XElement? documentation)
        : base(field, parentEntity, documentation) { }
}
