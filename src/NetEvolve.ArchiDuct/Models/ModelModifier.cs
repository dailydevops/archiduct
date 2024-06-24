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
    /// Marks the <see langword="type"/> or member as <see langword="abstract"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract"/>
    Abstract,

    /// <summary>
    /// Marks the member as <see langword="async"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async"/>
    Async,

    /// <summary>
    /// Marks the member as <see langword="extern"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern"/>
    Extern,

    /// <summary>
    /// Marks the member as <see langword="override"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override"/>
    Override,

    /// <summary>
    /// Marks the field as <see langword="const"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const"/>
    Const,

    /// <summary>
    /// Marks the field or <see langword="struct"/> as <see langword="readonly"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/readonly"/>
    ReadOnly,

    /// <summary>
    /// Marks the class as <see langword="sealed"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed"/>
    Sealed,

    /// <summary>
    /// Marks the class or method as <see langword="static"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static"/>
    Static,

    /// <summary>
    /// Marks the <see langword="type"/> or <see langword="method"/> as <see langword="partial"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-type"/>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-method"/>
    Partial,

    /// <summary>
    /// Marks the method as <see langword="new"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-modifier"/>
    New,

    /// <summary>
    /// Marks the method as <see langword="unsafe"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/unsafe"/>
    Unsafe,

    /// <summary>
    /// Marks the member as <see langword="ref"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref"/>
    Ref,

    /// <summary>
    /// Marks the member as <see langword="virtual"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual"/>
    Virtual,

    /// <summary>
    /// Marks the field as <see langword="volatile"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile"/>
    Volatile,

    /// <summary>
    /// Marks the property as <see langword="required"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required"/>
    Required,

    /// <summary>
    /// Marks the parameter as <see langword="in"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier"/>
    In,

    /// <summary>
    /// Marks the parameter as <see langword="out"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier"/>
    Out,

    /// <summary>
    /// Marks the parameter as <see langword="params"/>
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params"/>
    Params,

    /// <summary>
    /// Marks the parameter as extensible
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/this"/>
    This,

    /// <summary>
    /// Marks an parameter as optional
    /// </summary>
    Optional,
}
