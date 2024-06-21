namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Internals;
using static ICSharpCode.Decompiler.TypeSystem.Accessibility;

/// <summary>
/// Describes a member implementation. Could be a constructor, destructor, enum member, event, field, indexer, method, operator or property.
/// </summary>
public abstract class ModelMemberBase : ModelEntityBase
{
    /// <summary>
    /// Accessibility of the member.
    /// </summary>
    public ModelAccessibility Accessibility { get; }

    /// <summary>
    /// Collection of all modifiers for this type.
    /// </summary>
    public HashSet<ModelModifier> Modifiers { get; internal set; } = [];

    /// <summary>
    /// Returns the type id of the return type.
    /// </summary>
    public string? ReturnTypeId { get; }

    /// <inheritdoc />
    private protected ModelMemberBase(IMember member, ModelTypeBase parent, XElement? documentation)
        : base(member.GetIdString(), member.Name, member.GetQualifiedName(), parent, documentation)
    {
        Accessibility = MapAccessibility(member);
        ReturnTypeId = ModelFactory.GetReturnTypeId(member);
    }

    private static ModelAccessibility MapAccessibility(IMember member)
    {
        if (member.IsExplicitInterfaceImplementation)
        {
            return ModelAccessibility.None;
        }

        return member.Accessibility switch
        {
            Public => ModelAccessibility.Public,
            ProtectedAndInternal => ModelAccessibility.PrivateProtected,
            ProtectedOrInternal => ModelAccessibility.ProtectedInternal,
            Protected => ModelAccessibility.Protected,
            Internal => ModelAccessibility.Internal,
            Private => ModelAccessibility.Private,
            _ => ModelAccessibility.Internal,
        };
    }
}
