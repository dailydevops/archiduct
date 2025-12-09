namespace ArchiDuct.Abstractions;

/// <summary>
/// Represents a definition that asynchronously enumerates a collection of scopes.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="IScopeDefinition{T}"/> interface defines a contract for scope definitions that produce sequences of scopes
/// through asynchronous enumeration. This is useful for defining scope hierarchies or collections that are populated asynchronously.
/// </para>
/// <para>
/// Implementers must support the <see cref="IAsyncEnumerable{T}"/> protocol, allowing consumers to iterate over scopes
/// using the <c>await foreach</c> statement. This enables efficient streaming of scope instances without requiring
/// the entire collection to be available upfront.
/// </para>
/// </remarks>
/// <typeparam name="T">
/// The scope type parameter, constrained to implement <see cref="IScope{T}"/> where T is itself.
/// This ensures type safety and proper scope type preservation throughout enumeration.
/// </typeparam>
public interface IScopeDefinition<out T> : IAsyncEnumerable<T>
    where T : IScope<T>;
