namespace NetEvolve.ArchiDuct.Models;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a namespace implementation.
/// </summary>
public sealed class ModelNamespace : ModelEntityBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Namespace;

    /// <summary>
    /// Enumerable of all types within this namespace.
    /// </summary>
    /// <value>Enumerable of all described types.</value>
    public HashSet<string> Types { get; internal set; } = [];

    internal static readonly char[] _segmentSeparator = ['.'];

    internal ModelNamespace(string fullName, ModelAssembly parent, XElement? doc)
        : base(
            $"N:{fullName}",
            GetLastSegment(fullName),
            $"{parent.FullName}, {fullName}",
            parent,
            doc
        ) { }

    private static string GetLastSegment(string fullName) =>
        string.IsNullOrWhiteSpace(fullName)
            ? string.Empty
            : fullName.Split(_segmentSeparator, StringSplitOptions.RemoveEmptyEntries)[^1];
}
