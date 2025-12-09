namespace ArchiDuct.Abstractions;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Defines the contract for a dynamic Architecture Testing Scope.
/// </summary>
/// <remarks>
/// This interface represents a defined testing scope within the architecture testing framework.
/// Implementations of this interface define the boundaries and rules for validating architectural constraints
/// within a specific testing context. The interface is intentionally generic to allow for flexible scope implementations.
/// </remarks>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Generic Interface")]
public interface IScopeDefinition { }
