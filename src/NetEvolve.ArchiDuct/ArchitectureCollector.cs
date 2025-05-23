﻿namespace NetEvolve.ArchiDuct;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using NetEvolve.ArchiDuct.Abstractions;
using NetEvolve.ArchiDuct.Internals;
using NetEvolve.ArchiDuct.Models;
using NetEvolve.Arguments;
using NetEvolve.FluentValue;

/// <summary>
/// Entry point to collect the information about the architecture of a set of assemblies.
/// </summary>
public sealed class ArchitectureCollector : IArchitectureCollector
{
    private readonly HashSet<SourceAssembly> _registeredSources;

    private ArchitectureCollector() => _registeredSources = new HashSet<SourceAssembly>(5);

    /// <summary>
    /// Creates a new instance of <see cref="IArchitectureCollector"/>, which can be used to collect a set of one or more assembly types.
    /// </summary>
    public static IArchitectureCollector Create() => new ArchitectureCollector();

    /// <inheritdoc cref="IArchitectureCollector.AddAssembly(Assembly, bool)" />
    public IArchitectureCollector AddAssembly([NotNull] Assembly assembly, bool includeReferences = false)
    {
        Argument.ThrowIfNull(assembly);

        var source = new SourceAssembly(assembly);

        if (!_registeredSources.Add(source))
        {
            throw new InvalidOperationException($"The assembly '{assembly.FullName}' has already been registered.");
        }

        return this;
    }

    /// <inheritdoc cref="IArchitectureCollector.AddDirectory(string, bool, bool)" />
    public IArchitectureCollector AddDirectory(
        [NotNull] string directoryPath,
        bool includeSubDirectories = false,
        bool includeReferences = false
    )
    {
        Argument.ThrowIfNullOrWhiteSpace(directoryPath);

        var directory = new DirectoryInfo(directoryPath);

        if (!directory.Exists)
        {
            throw new DirectoryNotFoundException($"The directory '{directory.FullName}' does not exist.");
        }

        var options = new EnumerationOptions
        {
            RecurseSubdirectories = includeSubDirectories,
            MatchCasing = MatchCasing.CaseInsensitive,
            MatchType = MatchType.Simple,
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System,
            IgnoreInaccessible = true,
        };
        var catchedExceptions = new List<Exception>();
        var addedAssemblies = false;

        foreach (var candidate in directory.EnumerateFiles("*.*", options))
        {
            if (candidate.Extension is not ".dll" and not ".exe")
            {
                continue;
            }

            try
            {
#pragma warning disable S3885 // "Assembly.Load" should be used
                var assembly = Assembly.LoadFile(candidate.FullName);
#pragma warning restore S3885 // "Assembly.Load" should be used

                _ = AddAssembly(assembly, includeReferences);
                addedAssemblies = true;
            }
            catch (BadImageFormatException)
            {
                // Ignore invalid assemblies
            }
            catch (Exception ex)
            {
                catchedExceptions.Add(ex);
            }
        }

        if (catchedExceptions.Count > 0)
        {
            throw new AggregateException(
                $"Some assemblies could not be loaded from the directory '{directory.FullName}'.",
                catchedExceptions
            );
        }

        if (!addedAssemblies)
        {
            throw new InvalidOperationException($"No assemblies were found in the directory '{directory.FullName}'.");
        }

        return this;
    }

    /// <inheritdoc cref="IArchitectureCollector.FilterNamespace(IConstraint)" />
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="constraint"/> is <see langword="null"/>.</exception>
    public IArchitectureCollector FilterNamespace(IConstraint constraint)
    {
        Argument.ThrowIfNull(constraint);

        if (_registeredSources.Count == 0)
        {
            throw new ArgumentException("No sources were registered.", nameof(constraint));
        }

        foreach (var source in _registeredSources)
        {
            if (!source.Filters.Add(new SourceFilter(td => td.Namespace, constraint)))
            {
                throw new ArgumentException("SourceFilter already registered.", nameof(constraint));
            }
        }

        return this;
    }

    /// <inheritdoc cref="IArchitectureCollector.FilterType(Type)"/>
    public IArchitectureCollector FilterType(Type type)
    {
        Argument.ThrowIfNull(type);

        var assembly = type.Assembly;
        var typeFullName = GetTypeFullName(type);

        var sourceAssembly = new SourceAssembly(assembly);

        if (!_registeredSources.TryGetValue(sourceAssembly, out var source))
        {
            source = sourceAssembly;

            if (!_registeredSources.Add(source))
            {
                throw new InvalidOperationException($"The assembly '{assembly.FullName}' has already been registered.");
            }
        }

        var filter = new SourceFilter(td => td.ReflectionName, Value.Not.Null.And.EqualTo(typeFullName, Ordinal));
        if (!source.Filters.Add(filter))
        {
            throw new InvalidOperationException("SourceFilter already registered.");
        }

        return this;
    }

    internal static string GetTypeFullName(Type type)
    {
        if (!type.IsNested || type.DeclaringType is null)
        {
            return $"{type.Namespace}.{type.Name}";
        }

        var declaringType = type.DeclaringType;
        while (declaringType.IsNested && declaringType.DeclaringType is not null)
        {
            declaringType = declaringType.DeclaringType;
        }

        return $"{declaringType.Namespace}.{declaringType.Name}";
    }

    /// <inheritdoc cref="IArchitectureCollector.Collect" />
    public IArchitecture Collect()
    {
        if (_registeredSources.Count == 0)
        {
            throw new InvalidOperationException("No sources have been registered.");
        }

        var results = new List<ModelAssembly>();
        var decompilerSettings = new DecompilerSettings(LanguageVersion.Latest) { ThrowOnAssemblyResolveErrors = true };

        _ = Parallel.ForEach(
            _registeredSources,
            source =>
            {
                var result = CollectAssembly(source, decompilerSettings);

                if (result is { Count: > 0 })
                {
                    results.AddRange(result);
                }
            }
        );

        return new Architecture(results);
    }

    /// <inheritdoc cref="IArchitectureCollector.CollectAsync(CancellationToken)" />
    public async ValueTask<IArchitecture> CollectAsync(CancellationToken cancellationToken = default)
    {
        if (_registeredSources.Count == 0)
        {
            throw new InvalidOperationException("No sources have been registered.");
        }

        var results = new List<ModelAssembly>();
        var decompilerSettings = new DecompilerSettings(LanguageVersion.Latest) { ThrowOnAssemblyResolveErrors = true };

        await Parallel
            .ForEachAsync(
                _registeredSources,
                cancellationToken,
                (source, token) =>
                {
                    if (token.IsCancellationRequested)
                    {
                        return ValueTask.FromCanceled(token);
                    }

                    var result = CollectAssembly(source, decompilerSettings);

                    if (result is { Count: > 0 })
                    {
                        results.AddRange(result);
                    }

                    return ValueTask.CompletedTask;
                }
            )
            .ConfigureAwait(false);

        return new Architecture(results);
    }

    private static List<ModelAssembly> CollectAssembly(SourceAssembly source, DecompilerSettings decompilerSettings)
    {
        var assembly = source.Assembly;

        var results = new List<ModelAssembly>();
        var decompiler = new Decompiler(assembly.Location, decompilerSettings);

        var result = decompiler.Decompile(source.Filters);

        if (result.Count != 0)
        {
            results.AddRange(result);
        }

        return results;
    }
}
