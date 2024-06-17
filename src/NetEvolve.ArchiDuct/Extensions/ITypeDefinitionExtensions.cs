namespace NetEvolve.ArchiDuct.Extensions;

using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;

internal static class ITypeDefinitionExtensions
{
    public static IEnumerable<ITypeDefinition> EnumerateBaseTypeDefinitions(
        this ITypeDefinition typeDefinition
    ) => typeDefinition.GetAllBaseTypeDefinitions().Reverse().Skip(1);

    public static HashSet<string> GetAllBaseTypeIds(this ITypeDefinition typeDefinition)
    {
        var baseTypeDefinitions = typeDefinition.EnumerateBaseTypeDefinitions();
        if (typeDefinition.Kind != TypeKind.Interface)
        {
            baseTypeDefinitions = baseTypeDefinitions.Where(x => x.Kind != TypeKind.Interface);
        }
        var baseTypeIds = baseTypeDefinitions.Select(x => x.GetIdString());
        if (typeDefinition.Kind == TypeKind.Interface)
        {
            baseTypeIds = baseTypeIds.Where(x => !x.Equals("T:System.Object", OrdinalIgnoreCase));
        }

        return baseTypeIds.ToHashSet();
    }

    public static HashSet<string> GetAllImplementationIds(this ITypeDefinition typeDefinition)
    {
        if (typeDefinition.Kind is TypeKind.Enum or TypeKind.Interface or TypeKind.Delegate)
        {
            return [];
        }

        return typeDefinition
            .EnumerateBaseTypeDefinitions()
            .Where(x => x.Kind == TypeKind.Interface)
            .Select(x => x.GetIdString())
            .ToHashSet();
    }

    public static HashSet<string> GetAllInheritedMemberIds(this ITypeDefinition typeDefinition)
    {
        if (typeDefinition.Kind is TypeKind.Enum or TypeKind.Delegate)
        {
            return [];
        }

        var inheritedMemberIds = typeDefinition
            .EnumerateBaseTypeDefinitions()
            .Where(x => x.Kind != TypeKind.Interface)
            .SelectMany(x => x.Members)
            .Where(x => x is not null && !x.IsDefaultConstructor())
            .Select(x => x.GetIdString());

        if (typeDefinition.Kind == TypeKind.Interface)
        {
            inheritedMemberIds = inheritedMemberIds.Where(x =>
                !x.StartsWith("M:System.Object.", OrdinalIgnoreCase)
            );
        }

        return inheritedMemberIds.ToHashSet();
    }
}
