namespace ArchiDuct.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class ScopeUsageAttribute : Attribute
{
    public string? DefinitionName { get; }

    public ScopeUsageAttribute(string? definitionName = null) => DefinitionName = definitionName;
}
