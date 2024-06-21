namespace NetEvolve.ArchiDuct.Models;

/// <summary>
/// Described all accessibilities for types, methods, parameters and all the other thinks.
/// </summary>
public enum ModelAccessibility
{
    /// <summary>
    /// No modifiers
    /// </summary>
    None = 0,

    /// <summary>
    /// The entity is accessible only within the same code file.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/file"/>
    File,

    /// <summary>
    /// The entity is accessible everywhere.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/public"/>
    Public,

    /// <summary>
    /// The entity is only accessible within the same class and in derived classes.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected"/>
    Protected,

    /// <summary>
    /// The entity is accessible within the same assembly.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/internal"/>
    Internal,

    /// <summary>
    /// The entity is only accessible within the same class.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/private"/>
    Private,

    /// <summary>
    /// The entity is accessible within the same assembly and in derived classes.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/private-protected"/>
    PrivateProtected,

    /// <summary>
    /// The entity is accessible within the same assembly or by derived classes in another assembly.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected-internal"/>
    ProtectedInternal
}
