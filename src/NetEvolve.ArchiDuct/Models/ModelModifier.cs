namespace NetEvolve.ArchiDuct.Models;

/// <summary>
/// Described all modificators for types, methods, parameters and all the other thinks.
/// </summary>
public enum ModelModifier
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
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract"/>
    Abstract,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async"/>
    Async,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern"/>
    Extern,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override"/>
    Override,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly"/>
    ReadOnly,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed"/>
    Sealed,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static"/>
    Static,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-type"/>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-method"/>
    Partial,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-modifier"/>
    New,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/unsafe"/>
    Unsafe,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref"/>
    Ref,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual"/>
    Virtual,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile"/>
    Volatile,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required"/>
    Required,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier"/>
    In,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier"/>
    Out,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params"/>
    Params,

    /// <summary>
    /// TODO: Proper comments
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/this"/>
    This,

    /// <summary>
    /// Marks an parameter as optional.
    /// </summary>
    Optional,
}
