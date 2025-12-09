namespace ArchiDuct.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ScopeDefinitionAttribute : Attribute
{
    public string? DefinitionName { get; }

    public ScopeDefinitionAttribute(string? definitionName = null) => DefinitionName = definitionName;
}
