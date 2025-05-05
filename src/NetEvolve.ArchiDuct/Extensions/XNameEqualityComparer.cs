namespace NetEvolve.ArchiDuct.Extensions;

using System.Xml.Linq;

internal sealed class XNameEqualityComparer : IEqualityComparer<XName?>
{
    private static readonly Lazy<IEqualityComparer<XName?>> _instance = new Lazy<IEqualityComparer<XName?>>(() =>
        new XNameEqualityComparer()
    );

    public static IEqualityComparer<XName?> Instance => _instance.Value;

    private XNameEqualityComparer() { }

    public bool Equals(XName? x, XName? y) =>
        x == y || (x is not null && y is not null && x.LocalName.Equals(y.LocalName, Ordinal));

    public int GetHashCode(XName? obj) => obj?.GetHashCode() ?? default;
}
