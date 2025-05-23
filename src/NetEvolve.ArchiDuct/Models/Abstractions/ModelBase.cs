﻿namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Models;

/// <summary>
/// Represents a type or member description.
/// </summary>
public abstract class ModelBase
{
    private readonly ModelDocumentation? _documentation;

    /// <summary>
    /// Gets the xml for the described object.
    /// </summary>
    public ModelDocumentation? Documentation => _documentation?.IsValid() == true ? _documentation : null;

    /// <summary>
    /// Gets the fully qualified name of the class the return type is pointing to.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Unique ID string that identifies the type or member.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
    public string Id { get; }

    /// <summary>
    /// Defines the described object type.
    /// </summary>
    public abstract ModelKind Kind { get; }

    /// <summary>
    /// Gets the short name of the class the return type is pointing to.
    /// </summary>
    /// <returns>
    /// "Int32[]" for int[]<br/>
    /// "List" for List&lt;string&gt;
    /// "SpecialFolder" for Environment.SpecialFolder
    /// </returns>
    public string Name { get; }

    private protected ModelBase(string id, string name, string fullName, XElement? xml)
        : this(id, name, fullName, ModelDocumentation.Default(xml)) { }

    private protected ModelBase(string id, string name, string fullName, ModelDocumentation? doc)
    {
        Id = id;
        Name = name;
        FullName = fullName;

        _documentation = doc;
    }
}
