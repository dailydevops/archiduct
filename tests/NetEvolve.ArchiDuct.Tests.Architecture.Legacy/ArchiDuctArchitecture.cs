namespace NetEvolve.ArchiDuct.Tests.Architecture.Legacy;

using System;

internal static class ArchiDuctArchitecture
{
    private static readonly Lazy<ArchUnitNET.Domain.Architecture> _instance = new(
        LoadArchitecture,
        System.Threading.LazyThreadSafetyMode.PublicationOnly
    );

    public static ArchUnitNET.Domain.Architecture Instance => _instance.Value;

    private static ArchUnitNET.Domain.Architecture LoadArchitecture() =>
        new ArchUnitNET.Loader.ArchLoader()
            .LoadAssemblies(typeof(ArchitectureCollector).Assembly)
            .Build();
}
