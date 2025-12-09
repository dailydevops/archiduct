namespace ArchiDuct.Abstractions;

/// <summary>
/// Represents a scope that can be nested and managed within a scope hierarchy.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="IScope{T}"/> interface defines a contract for scope objects that participate in scope hierarchies.
/// Scopes are typically used to manage contextual information, resource lifetimes, and logical groupings within an application.
/// </para>
/// </remarks>
/// <typeparam name="T">
/// The scope type parameter, constrained to implement <see cref="IScope{T}"/> itself.
/// This self-referential constraint enables proper type preservation in scope hierarchies.
/// </typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Design marker interface for scope types.")]
public interface IScope<out T> where T : IScope<T>;
