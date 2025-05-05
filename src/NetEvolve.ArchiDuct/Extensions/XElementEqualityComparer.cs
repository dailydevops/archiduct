namespace NetEvolve.ArchiDuct.Extensions;

using System.Xml.Linq;

internal sealed class XElementEqualityComparer : IEqualityComparer<XElement?>
{
    private static readonly Lazy<IEqualityComparer<XElement?>> _instance = new Lazy<IEqualityComparer<XElement?>>(() =>
        new XElementEqualityComparer()
    );

    public static IEqualityComparer<XElement?> Instance => _instance.Value;

    private XElementEqualityComparer() { }

    /// <inheritdoc/>
    public bool Equals(XElement? x, XElement? y)
    {
        if (x == y)
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        var sortedX = x.Sort();
        var sortedY = y.Sort();

        return sortedX is not null
            && sortedY is not null
            && sortedX.Name == sortedY.Name
            && sortedX.Attributes().SequenceEqual(sortedY.Attributes(), XAttributeEqualityComparer.Instance)
            && sortedX.Elements().SequenceEqual(sortedY.Elements(), this);
    }

    /// <inheritdoc/>
    public int GetHashCode(XElement? obj) => obj?.GetHashCode() ?? default;
}
