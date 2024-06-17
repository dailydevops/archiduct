﻿namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Members;

/// <summary>
/// Represents an type description.
/// </summary>
public abstract class ModelTypeBase : ModelEntityBase
{
    private readonly Dictionary<string, ModelMemberBase> _members;

    /// <summary>
    /// Enumerable of all base type ids.
    /// </summary>
    public HashSet<string> BaseTypes { get; internal set; } = [];

    /// <summary>
    /// Enumerable of all derived type ids.
    /// </summary>
    public HashSet<string> DerivedTypes { get; internal set; } = [];

    /// <summary>
    /// Enumerable of all interface implementation ids.
    /// </summary>
    public HashSet<string> Implementations { get; internal set; } = [];

    /// <summary>
    /// Enumerable of all inherited member ids.
    /// </summary>
    public HashSet<string> InheritedMembers { get; internal set; } = [];

    /// <summary>
    /// If true, the type is a nested type. Which has no namespace as parent, but a parent type.
    /// </summary>
    public bool IsNested { get; }

    /// <summary>
    /// Collection of all members within this type.
    /// </summary>
    /// <value>Read-only collection of all described members.</value>
    public IEnumerable<ModelMemberBase> Members => _members.Values;

    /// <summary>
    /// Collection of all modifiers for this type.
    /// </summary>
    public HashSet<ModelModifier> Modifiers { get; internal set; } = [];

    /// <summary>
    /// Unique ID string that identifies the namespace.
    /// </summary>
    public string NamespaceId { get; set; } = default!;

    /// <summary>
    /// Collection of all nested types.
    /// </summary>
    public HashSet<string> NestedTypes { get; internal set; } = [];

    /// <summary>
    /// Collection of all type parameters within this type.
    /// </summary>
    /// <value>Read-only collection of all described members.</value>
    public HashSet<ModelTypeParameter> TypeParameters { get; internal set; } = default!;

    private protected ModelTypeBase(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(
            typeDefinition?.GetIdString()
                ?? throw new ArgumentNullException(nameof(typeDefinition)),
            typeDefinition.Name,
            typeDefinition.FullName,
            parentEntity,
            documentation
        )
    {
        _members = new Dictionary<string, ModelMemberBase>(StringComparer.Ordinal);

        IsNested = typeDefinition.FullTypeName.IsNested;
    }

    internal void AddMember(ModelMemberBase member) => _members[member.Id] = member;
}
