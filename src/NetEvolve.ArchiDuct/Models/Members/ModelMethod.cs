namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a method implementation.
/// </summary>
public class ModelMethod : ModelMemberAdvancedBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Method;

    /// <inheritdoc />
    internal ModelMethod(IMethod method, ModelTypeBase parentEntity, XElement? documentation)
        : base(method, parentEntity, documentation) { }
}
