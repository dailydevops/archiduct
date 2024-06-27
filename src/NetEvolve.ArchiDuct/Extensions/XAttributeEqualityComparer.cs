namespace NetEvolve.ArchiDuct.Extensions;

using System.Xml.Linq;

// TODO: separat package
internal sealed class XAttributeEqualityComparer : IEqualityComparer<XAttribute?>
{
    private static readonly Lazy<IEqualityComparer<XAttribute?>> _instance = new Lazy<
        IEqualityComparer<XAttribute?>
    >(() => new XAttributeEqualityComparer());

    public static IEqualityComparer<XAttribute?> Instance => _instance.Value;

    private XAttributeEqualityComparer() { }

    /// <inheritdoc/>
    public bool Equals(XAttribute? x, XAttribute? y) =>
        x == y
        || (
            x is not null
            && y is not null
            && XNameEqualityComparer.Instance.Equals(x.Name, y.Name)
            && x.Value.Equals(y.Value, Ordinal)
        );

    /// <inheritdoc/>
    public int GetHashCode(XAttribute? obj) => obj?.GetHashCode() ?? default;
}
