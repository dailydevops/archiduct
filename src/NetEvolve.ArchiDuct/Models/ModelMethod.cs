namespace NetEvolve.ArchiDuct.Models;

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

    /// <summary>
    /// Indicates whether the method is a local function.
    /// </summary>
    public bool IsLocal { get; }

    /// <inheritdoc />
    internal ModelMethod(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) => IsLocal = method.IsLocalFunction;
}
