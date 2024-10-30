namespace NetEvolve.ArchiDuct.Tests.PublicApi;

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using PublicApiGenerator;
using VerifyXunit;
using Xunit;

public class ArchiDuctPublicApiTests
{
    [Fact]
    public Task PublicApi_HasNotChanged_Expected()
    {
        var assembly = typeof(ArchitectureCollector).Assembly;
        var types = assembly.GetTypes().Where(IsVisibleToIntelliSense).ToArray();

        var options = new ApiGeneratorOptions
        {
            ExcludeAttributes =
            [
                typeof(InternalsVisibleToAttribute).FullName!,
                "System.Runtime.CompilerServices.IsByRefLikeAttribute",
                typeof(TargetFrameworkAttribute).FullName!,
                typeof(CLSCompliantAttribute).FullName!,
                typeof(AssemblyMetadataAttribute).FullName!,
                typeof(NeutralResourcesLanguageAttribute).FullName!,
                typeof(AttributeUsageAttribute).FullName!,
            ],
            IncludeTypes = types,
        };

        var publicApi = assembly.GeneratePublicApi(options);

        return Verifier.Verify(publicApi);
    }

    private static bool IsVisibleToIntelliSense(Type type)
    {
        var browsable = type.GetCustomAttribute<BrowsableAttribute>();
        if (browsable is null || browsable.Browsable)
        {
            return true;
        }

        var editorBrowsable = type.GetCustomAttribute<EditorBrowsableAttribute>();
        return editorBrowsable is null || editorBrowsable.State != EditorBrowsableState.Never;
    }
}
