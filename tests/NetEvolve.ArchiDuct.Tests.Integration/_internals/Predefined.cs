namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using NetEvolve.ArchiDuct.Models.Documentation;
using VerifyTests;
using VerifyXunit;

internal static class Predefined
{
    [ModuleInitializer]
    public static void Init()
    {
        Verifier.DerivePathInfo(
            (sourceFile, projectDirectory, method, type) =>
            {
                var snapshots = Path.Combine(projectDirectory, "_snapshots");
                _ = Directory.CreateDirectory(snapshots);
                return new(snapshots, type.Name, method.Name);
            }
        );

        VerifierSettings.AutoVerify(includeBuildServer: false);
        VerifierSettings.SortJsonObjects();
        VerifierSettings.SortPropertiesAlphabetically();

        VerifierSettings.UniqueForTargetFrameworkAndVersion();
        VerifierSettings.HashParameters();

        VerifierSettings.IgnoreMembersWithType<Version>();

        // TODO: Workaround: Until the documentation XML files are also played out on Non-Windows system.
        VerifierSettings.IgnoreMembersWithType<ModelDocumentation>();
    }
}
