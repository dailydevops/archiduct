namespace NetEvolve.ArchiDuct.Extensions;

using NetEvolve.ArchiDuct.Models.Abstractions;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlAttributeConstants;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlPropertyConstants;

internal static class ModelExtensions
{
    public static string? GetParameterDocumentation(this ModelBase model, string parameterName) =>
        model
            .GetElements(Param)
            ?.FirstOrDefault(p => string.Equals(p.Attribute(Name)?.Value, parameterName, Ordinal))
            .GetElementValue();

    public static string? GetTypeParameterDocumentation(
        this ModelBase model,
        string parameterName
    ) =>
        model
            .GetElements(TypeParam)
            ?.FirstOrDefault(p => string.Equals(p.Attribute(Name)?.Value, parameterName, Ordinal))
            .GetElementValue();
}
