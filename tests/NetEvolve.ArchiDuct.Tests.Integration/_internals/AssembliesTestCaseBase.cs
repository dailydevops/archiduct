﻿namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

public abstract class AssembliesTestCaseBase<TTestCase>(GenericTypeProvider<TTestCase> provider)
    : TestCaseBase<GenericTypeProvider<TTestCase>>(provider, disableMembersCheck: true)
    where TTestCase : notnull
{
    [SkippableFact]
    public async Task Verify_Architecture()
    {
        Skip.If(IsCIExecution, "Disabled in CI for now.");

        var architecture = _provider.Architecture;
        _ = await Verifier.Verify(architecture);
    }

    [SkippableFact]
    public async Task Verify_Assemblies()
    {
        Skip.If(IsCIExecution, "Disabled in CI for now.");

        var architecture = _provider.Architecture;

        _ = Parallel.ForEach(
            architecture.Assemblies,
            assembly =>
            {
                assembly.Value.Types.Clear();
                assembly.Value.Members.Clear();
            }
        );
        _ = await Verifier.Verify(architecture.Assemblies);
    }
}
