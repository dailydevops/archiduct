namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using NetEvolve.ArchiDuct.Models;
using NetEvolve.ArchiDuct.Models.Abstractions;
using VerifyTests;
using VerifyTUnit;

internal static class Predefined
{
    [ModuleInitializer]
    public static void Init()
    {
        Verifier.DerivePathInfo(
            (__, projectDirectory, type, method) =>
            {
                var directory = Path.Combine(projectDirectory, "_snapshots", Namer.TargetFrameworkNameAndVersion);
                _ = Directory.CreateDirectory(directory);
                return new(directory, type.Name, method.Name);
            }
        );

        VerifierSettings.AutoVerify(includeBuildServer: false);
        VerifierSettings.SortJsonObjects();
        VerifierSettings.SortPropertiesAlphabetically();

        VerifierSettings.OrderEnumerableBy<ModelAttribute>(model => model.Id);
        VerifierSettings.OrderEnumerableBy<ModelMemberBase>(model => model.Id);
        VerifierSettings.OrderEnumerableBy<ModelNamespace>(model => model.Id);
        VerifierSettings.OrderEnumerableBy<ModelReference>(model => model.Id);
        VerifierSettings.OrderEnumerableBy<ModelTypeBase>(model => model.Id);

        VerifierSettings.IgnoreMembersWithType<Version>();

        // Workaround: Until the documentation XML files are also played out on Non-Windows system.
        VerifierSettings.IgnoreMembersWithType<ModelDocumentation>();

        VerifierSettings.AddScrubber(ReplaceVersions);
    }

    private static void ReplaceVersions(StringBuilder builder)
    {
        const string version = "Version=";

        if (builder.Length <= version.Length)
        {
            return;
        }

        var startIndex = 0;
        do
        {
            var value = builder.ToString();
            var versionIndex = value.IndexOf(version, startIndex, StringComparison.OrdinalIgnoreCase);
            if (versionIndex == -1)
            {
                return;
            }

            var endIndex = value.IndexOf(',', versionIndex);
            if (endIndex == -1)
            {
                return;
            }
            _ = builder.Remove(versionIndex, endIndex - versionIndex).Insert(versionIndex, $"{version}x.x.x.x");
            startIndex = versionIndex + version.Length;
        } while (startIndex < builder.Length);
    }
}
