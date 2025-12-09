namespace NetEvolve.ArchiDuct.Attributes;

/// <summary>
/// Marks a method or property that defines a dynamic Architecture Testing Scope.
/// </summary>
/// <remarks>
/// This attribute is used to identify members that provide scope definitions for architectural testing.
/// Scopes define the boundary and rules for architecture validation within a test context.
/// </remarks>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ScopeDefinitionAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the scope definition.
    /// </summary>
    /// <value>
    /// The custom name for the scope definition, or <see langword="null"/> if no name was specified.
    /// </value>
    public string? DefinitionName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeDefinitionAttribute"/> class.
    /// </summary>
    /// <param name="definitionName">
    /// The optional name for the scope definition. If not specified, defaults to <see langword="null"/>.
    /// </param>
    public ScopeDefinitionAttribute(string? definitionName = null) => DefinitionName = definitionName;
}
