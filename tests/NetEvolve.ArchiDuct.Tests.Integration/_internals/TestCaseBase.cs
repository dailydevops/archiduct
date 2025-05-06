namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NetEvolve.ArchiDuct.Models;

public abstract class TestCaseBase<TTypeProvider>(
    TTypeProvider provider,
    bool disableMembersCheck = false,
    bool disableTypesCheck = false,
    bool enableDocumentationCheck = false,
    OSPlatform[]? operationSystems = null
)
    where TTypeProvider : notnull, TypeProviderBase
{
    private protected readonly TTypeProvider _provider = provider;
    private readonly bool _disableMembersCheck = disableMembersCheck;
    private readonly bool _disableTypesCheck = disableTypesCheck;
    private readonly bool _enableDocumentationCheck = enableDocumentationCheck;
    private readonly OSPlatform[]? _operationSystems = operationSystems;

    protected bool IsOperationSystemUnsupported =>
        _operationSystems is not null && !Array.TrueForAll(_operationSystems, RuntimeInformation.IsOSPlatform);

    [Test]
    public async ValueTask Instance_AssemblyOne_Expected()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");

        _ = await Assert.That(_provider.Architecture.Assemblies).HasSingleItem();
    }

    [Test]
    public async ValueTask Instance_MembersNotEmpty_Expected()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.When(_disableMembersCheck, "Members check is disabled.");

        _ = await Assert.That(_provider.Architecture.Members).IsNotEmpty();
    }

    [Test]
    public async ValueTask Instance_NamespacesOne_Expected()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");

        _ = await Assert.That(_provider.Architecture.Namespaces).HasSingleItem();
    }

    [Test]
    public async ValueTask Instance_TypesNotEmpty_Expected()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.When(_disableTypesCheck, "Types check is disabled.");

        _ = await Assert.That(_provider.Architecture.Types).IsNotEmpty();
    }

    [Test]
    public async Task Verify_Members()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.When(_disableMembersCheck, "Members check is disabled.");
        var members = _provider.Architecture.Members;
        Skip.When(members.Count == 0, "Members are empty.");

        var verify = Verify(members).IgnoreParameters();

        if (_enableDocumentationCheck)
        {
            verify = verify.IgnoreMembersWithType<ModelAttribute>().AlwaysIncludeMembersWithType<ModelDocumentation>();
        }

        _ = await verify;
    }

    [Test]
    public async Task Verify_Types()
    {
        Skip.When(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.When(_disableTypesCheck, "Types check is disabled.");
        var types = _provider.Architecture.Types;
        Skip.When(types.Count == 0, "Types are empty.");

        var verify = Verify(types).IgnoreParameters();

        if (_enableDocumentationCheck)
        {
            verify = verify.IgnoreMembersWithType<ModelAttribute>().AlwaysIncludeMembersWithType<ModelDocumentation>();
        }

        _ = await verify;
    }
}
