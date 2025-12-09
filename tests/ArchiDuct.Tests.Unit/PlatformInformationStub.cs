namespace ArchiDuct.Tests.Unit;

using System;
using Microsoft.Testing.Platform.Services;

internal sealed class PlatformInformationStub : IPlatformInformation
{
    public PlatformInformationStub(string name, string version)
    {
        Name = name;
        Version = version;
        BuildDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        CommitHash = "deadbeef";
    }

    public string Name { get; }

    public string Version { get; }

    public DateTimeOffset? BuildDate { get; }

    public string CommitHash { get; }
}
