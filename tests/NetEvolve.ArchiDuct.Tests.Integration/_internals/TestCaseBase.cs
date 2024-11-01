namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

public abstract class TestCaseBase<TTypeProvider>(
    TTypeProvider provider,
    bool disableMembersCheck = false,
    bool disableTypesCheck = false,
    OSPlatform[]? operationSystems = null
) : IClassFixture<TTypeProvider>
    where TTypeProvider : notnull, TypeProviderBase
{
    private protected readonly TTypeProvider _provider = provider;
    private readonly bool _disableMembersCheck = disableMembersCheck;
    private readonly bool _disableTypesCheck = disableTypesCheck;
    private readonly OSPlatform[]? _operationSystems = operationSystems;

    protected bool IsCIExecution =>
        Environment.GetEnvironmentVariable("CI") is string ci
        && ci.Equals("true", StringComparison.OrdinalIgnoreCase);

    protected bool IsOperationSystemUnsupported =>
        _operationSystems is not null && !_operationSystems.All(RuntimeInformation.IsOSPlatform);

    [SkippableFact]
    public void Instance_AssemblyOne_Expected()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");

        _ = Assert.Single(_provider.Architecture.Assemblies);
    }

    [SkippableFact]
    public void Instance_MembersNotEmpty_Expected()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.If(_disableMembersCheck, "Members check is disabled.");

        Assert.NotEmpty(_provider.Architecture.Members);
    }

    [SkippableFact]
    public void Instance_NamespacesOne_Expected()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");

        Assert.Single(_provider.Architecture.Namespaces);
    }

    [SkippableFact]
    public void Instance_TypesNotEmpty_Expected()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.If(_disableTypesCheck, "Types check is disabled.");

        Assert.NotEmpty(_provider.Architecture.Types);
    }

    [SkippableFact]
    public async Task Verify_Members()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.If(_disableMembersCheck, "Members check is disabled.");
        var members = _provider.Architecture.Members;
        Skip.If(members.Count == 0, "Members are empty.");

        _ = await Verifier.Verify(members);
    }

    [SkippableFact]
    public async Task Verify_Types()
    {
        Skip.If(IsOperationSystemUnsupported, "Operation system is not supported.");
        Skip.If(_disableTypesCheck, "Types check is disabled.");
        var types = _provider.Architecture.Types;
        Skip.If(types.Count == 0, "Types are empty.");

        _ = await Verifier.Verify(types);
    }
}
