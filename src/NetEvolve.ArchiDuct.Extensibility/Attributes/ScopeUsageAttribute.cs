namespace NetEvolve.ArchiDuct.Attributes;

/// <summary>
/// Marks a method that uses a dynamic Architecture Testing Scope.
/// </summary>
/// <remarks>
/// This attribute is used to associate a method with one or more scope definitions.
/// Multiple scope usages can be applied to a single method, allowing complex architectural testing scenarios.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class ScopeUsageAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the scope definition being used.
    /// </summary>
    /// <value>
    /// The name of the scope definition to use, or <see langword="null"/> if no specific name was specified.
    /// </value>
    public string? DefinitionName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeUsageAttribute"/> class.
    /// </summary>
    /// <param name="definitionName">
    /// The optional name of the scope definition to use. If not specified, defaults to <see langword="null"/>.
    /// </param>
    public ScopeUsageAttribute(string? definitionName = null) => DefinitionName = definitionName;
}
