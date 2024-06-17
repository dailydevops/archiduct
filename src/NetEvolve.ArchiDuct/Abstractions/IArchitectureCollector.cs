namespace NetEvolve.ArchiDuct.Abstractions;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NetEvolve.FluentValue;

/// <summary>
/// The <see cref="IArchitectureCollector"/> interface provides the functionality to register one or more assemblies to be collected.
/// </summary>
public interface IArchitectureCollector
{
    /// <summary>
    /// Adds the give assembly to the list of assemblies to be collected.
    /// </summary>
    /// <param name="assembly">
    /// The assembly to be collected.
    /// </param>
    /// <param name="includeReferences">
    /// If <c><see langword="true"/></c>, the references of the assembly will also be collected.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector AddAssembly([NotNull] Assembly assembly, bool includeReferences = false);

    /// <summary>
    /// Adds the assembly of the given type to the list of assemblies to be collected.
    /// </summary>
    /// <param name="type">
    /// The type of the assembly to be collected.
    /// </param>
    /// <param name="includeReferences">
    /// If <c><see langword="true"/></c>, the references of the assembly will also be collected.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector AddAssembly([NotNull] Type type, bool includeReferences = false) =>
        AddAssembly(type.Assembly, includeReferences);

    /// <summary>
    /// Adds the assembly of the given type to the list of assemblies to be collected.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the assembly to be collected.
    /// </typeparam>
    /// <param name="includeReferences">
    /// If <c><see langword="true"/></c>, the references of the assembly will also be collected.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector AddAssembly<T>(bool includeReferences = false)
        where T : notnull => AddAssembly(typeof(T).Assembly, includeReferences);

    /// <summary>
    /// Adds one or more assemblies from the given directory to the list of assemblies to be collected.
    /// </summary>
    /// <param name="directoryPath">
    /// The path to the directory containing the assemblies to be collected.
    /// </param>
    /// <param name="includeSubDirectories">
    /// If <c><see langword="true"/></c>, the subdirectories of the directory will also be searched for assemblies.
    /// </param>
    /// <param name="includeReferences">
    /// If <c><see langword="true"/></c>, the references of the assemblies will also be collected.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector AddDirectory(
        [NotNull] string directoryPath,
        bool includeSubDirectories = false,
        bool includeReferences = false
    );

    /// <summary>
    /// Adds a namespace filter to all previously added assemblies. If the filter is applied, only the types that match the filter will be collected.
    /// </summary>
    /// <param name="constraint">
    /// The constraint to filter the namespaces.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector FilterNamespace(IConstraint constraint);

    /// <summary>
    /// Adds a <see cref="Type"/> filter to the assembly. If the filter is applied, only the types that match the filter will be collected.
    /// </summary>
    /// <param name="type">
    /// The type to filter.
    /// </param>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector FilterType(Type type);

    /// <summary>
    /// Adds a <see cref="Type"/> filter to the assembly. If the filter is applied, only the types that match the filter will be collected.
    /// </summary>
    /// <typeparam name="T">
    /// The type to filter.
    /// </typeparam>
    /// <returns>
    /// The current instance of the <see cref="IArchitectureCollector"/>.
    /// </returns>
    IArchitectureCollector FilterType<T>()
        where T : notnull => FilterType(typeof(T));

    /// <summary>
    /// Executes the collection of the assembly information and types.
    /// </summary>
    /// <returns>
    /// The collected results of the registered assemblies.
    /// </returns>
    IArchitecture Collect();

    /// <summary>
    /// Executes the collection of the assembly information and types asynchronously.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// The collected results of the registered assemblies.
    /// </returns>
    ValueTask<IArchitecture> CollectAsync(CancellationToken cancellationToken = default);
}
