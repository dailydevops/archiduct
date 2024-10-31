namespace NetEvolve.ArchiDuct.Models;

/// <summary>
/// Describes a <see langword="return"/> type.
/// </summary>
public sealed class ModelReturn
{
    /// <summary>
    /// Gets the return type identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets whether the return type is nullable.
    /// </summary>
    public bool IsNullable { get; }

    /// <summary>
    /// Gets whether the return type is <see langword="ref readonly"/>.
    /// </summary>
    public bool? IsRefReadonly { get; }

    internal ModelReturn(string id, bool isNullable, bool? isRefReadonly = null)
    {
        Id = id;
        IsNullable = isNullable;
        IsRefReadonly = isRefReadonly;
    }

    /// <summary>
    /// <see langword="Return"/> type is <see langword="void"/>.
    /// </summary>
    public static ModelReturn Void => new ModelReturn("T:System.Void", false);
}
