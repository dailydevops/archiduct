namespace NetEvolve.ArchiDuct.Internals;

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Resolver;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Models;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Members;
using NetEvolve.ArchiDuct.Models.Types;
using static NetEvolve.ArchiDuct.Constants;

internal sealed partial class Decompiler : IDisposable
{
    private static readonly ConcurrentDictionary<
        IModule,
        IDocumentationProvider
    > _documentationProviders = new ConcurrentDictionary<IModule, IDocumentationProvider>(
        new ModuleEqualityComparer()
    );

    private static readonly string[] _ignoredElements =
    [
        DocumentationXmlPropertyConstants.Example,
        DocumentationXmlPropertyConstants.Remarks,
        DocumentationXmlPropertyConstants.Returns,
        DocumentationXmlPropertyConstants.SeeAlso,
        DocumentationXmlPropertyConstants.Summary
    ];

    private readonly CSharpDecompiler _decompiler;

    private readonly List<ModelNamespace> _modelNamespaces;

    private readonly List<ModelTypeBase> _modelTypes;

    private readonly CSharpResolver _resolver;

    private bool _disposedValue;

    private ModelAssembly _modelAssembly = default!;

    public Decompiler(string assemblyFile, DecompilerSettings decompilerSettings)
    {
        _modelNamespaces = [];
        _modelTypes = [];

        _decompiler = new CSharpDecompiler(assemblyFile, decompilerSettings);
        _resolver = new CSharpResolver(_decompiler.TypeSystem);
    }

    /// <inheritdoc />
    public void Dispose() => Dispose(disposing: true);

    public ModelAssembly Decompile(HashSet<SourceFilter> filters)
    {
        var typeSystem = _decompiler.TypeSystem;
        var mainModule = typeSystem.MainModule;

        if (!TryGetDocumentationProvider(mainModule, out var documentationProvider))
        {
            throw new InvalidOperationException("Unable to load documentation provider.");
        }

        _decompiler.DocumentationProvider = documentationProvider;

        return DecompileModule(mainModule, filters);
    }

    private ModelAssembly DecompileModule(IModule module, HashSet<SourceFilter> filters)
    {
        _modelAssembly = new ModelAssembly(
            module,
            GetDocumentation($"T:{module.AssemblyName}.{AssemblyDoc}")
        );
        // TODO: Add GitVersion informations
        // TODO: Assembly References
        // TODO: Assembly Attributes


        DescribeTypeModels(module, filters);

        SetReferences();

        return _modelAssembly;
    }

    private static XElement? ConvertToDocumentation(string? documentationString) =>
        string.IsNullOrWhiteSpace(documentationString)
            ? null
            : XElement.Parse($"<doc>{documentationString}</doc>");

    private static bool IsDocumentationHelperClass(string name) =>
        name.EndsWith(AssemblyDoc, Ordinal) || name.EndsWith(NamespaceDoc, Ordinal);

    private static bool IsModuleClass(ITypeDefinition typeDefinition) =>
        typeDefinition.Kind == TypeKind.Class && typeDefinition.Name.Equals("<Module>", Ordinal);

    private static bool IsTypeDefinitionExcluded(ITypeDefinition typeDefinition) =>
        IsDocumentationHelperClass(typeDefinition.Name)
        || typeDefinition.IsCompilerGeneratedOrIsInCompilerGeneratedClass()
        || IsModuleClass(typeDefinition);

    private static void MapModelMemberOverloads(ModelTypeBase describedModel)
    {
        foreach (var member in describedModel.Members)
        {
            if (member is not ModelMemberAdvancedBase overloadable)
            {
                continue;
            }

            overloadable.OverloadedMembers = describedModel
                .Members.Where(x =>
                    member.Kind == x.Kind
                    && !member.Id.Equals(x.Id, Ordinal)
                    && member.Name.Equals(x.Name, Ordinal)
                )
                .Select(x => x.Id)
                .ToArray();
        }
    }

    private static void MapModelMemberParameters(IMember member, ModelMemberBase result)
    {
        if (result is ModelMemberAdvancedBase modelWithParameters)
        {
            if (member is IMethod genericMethod && genericMethod.TypeParameters.Any())
            {
                modelWithParameters.TypeParameters = genericMethod
                    .TypeParameters.Select(p => new ModelTypeParameter(p, modelWithParameters))
                    .ToArray();
            }

            if (
                member is IParameterizedMember parameterizedMember
                && parameterizedMember.Parameters.Any()
            )
            {
                modelWithParameters.Parameters = parameterizedMember
                    .Parameters.Select(p => new ModelParameter(p, modelWithParameters))
                    .ToArray();
            }
        }

        if (
            result is ModelIndexer modelIndexer
            && member is IParameterizedMember parameterized
            && parameterized.Parameters.Any()
        )
        {
            modelIndexer.Parameters = parameterized
                .Parameters.Select(p => new ModelParameter(p, modelIndexer))
                .ToArray();
        }
    }

    private static void MapModelTypeParameters(ITypeDefinition typeDefinition, ModelTypeBase model)
    {
        if (typeDefinition.TypeParameters.Any())
        {
            model.TypeParameters = typeDefinition
                .TypeParameters.Select(p => new ModelTypeParameter(p, model))
                .ToArray();
        }
    }

    private static bool TryGetDocumentationProvider(
        IModule module,
        [NotNullWhen(true)] out IDocumentationProvider? documentationProvider
    )
    {
        if (!_documentationProviders.TryGetValue(module, out documentationProvider))
        {
            documentationProvider =
                XmlDocLoader.LoadDocumentation(module.PEFile) ?? XmlDocLoader.MscorlibDocumentation;
            return _documentationProviders.TryAdd(module, documentationProvider);
        }

        return true;
    }

    private void DescribeMemberModel(IMember member, ModelTypeBase parentModel)
    {
        if (member.IsCompilerGeneratedOrIsInCompilerGeneratedClass())
        {
            return;
        }

        if (ShouldNotBeDescribedAccessModifier(member))
        {
            return;
        }

        if (!TryGetDocumentation(member, out var documentation))
        {
            //TODO: Include undocumented items?
        }

        if (member.IsDefaultConstructor() && documentation is null)
        {
            // TODO: Logging?
            return;
        }

        if (
            parentModel is ModelEnum
            && member is IField enumField
            && enumField.Name.Equals("value__", Ordinal)
        )
        {
            return;
        }

        var describedMember = ModelFactory.CreateModelMemberType(
            member,
            parentModel,
            documentation
        );
        MapModelMemberParameters(member, describedMember);

        // TODO: Describe Member Accessor - event add, invoke or remove
        // TODO: Describe Member Accessor - property getter or setter

        parentModel.AddMember(describedMember);
    }

    private void DescribeMemberModels(ITypeDefinition typeDefinition, ModelTypeBase describedModel)
    {
        if (typeDefinition.Kind != TypeKind.Delegate)
        {
            foreach (var member in typeDefinition.Members)
            {
                DescribeMemberModel(member, describedModel);
            }

            MapModelMemberOverloads(describedModel);
        }
    }

    private void DescribeTypeModel(
        ITypeDefinition typeDefinition,
        ModelEntityBase? parentEntity = null
    )
    {
        if (IsTypeDefinitionExcluded(typeDefinition))
        {
            return;
        }

        if (TryGetDocumentation(typeDefinition, out var documentation))
        {
            //TODO: Include undocumented items?
        }

        var ns = GetOrAddNamespace(typeDefinition.Namespace);

        if (parentEntity is null)
        {
            parentEntity = ns;
        }

        var describedModel = ModelFactory.CreateModelType(
            typeDefinition,
            parentEntity,
            documentation
        );

        MapModelTypeParameters(typeDefinition, describedModel);
        DescribeMemberModels(typeDefinition, describedModel);

        _modelTypes.Add(describedModel);

        foreach (var nestedTypeDefinition in typeDefinition.NestedTypes)
        {
            DescribeTypeModel(nestedTypeDefinition, describedModel);
        }
    }

    private void DescribeTypeModels(IModule module, HashSet<SourceFilter> filters)
    {
        foreach (var typeDefinition in module.TopLevelTypeDefinitions)
        {
            if (filters.Count != 0 && !filters.All(filter => ApplyFilter(filter, typeDefinition)))
            {
                continue;
            }

            DescribeTypeModel(typeDefinition);
        }
    }

    private static bool ApplyFilter(SourceFilter filter, ITypeDefinition typeDefinition)
    {
        var value = filter.ValueSelector.Invoke(typeDefinition);

        return filter.Constraint.IsSatisfiedBy(value);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    private XElement? GetDocumentation(string id)
    {
        var entity = IdStringProvider.FindEntity(id, _resolver);

        if (TryGetDocumentation(entity, out var documentation))
        {
            return documentation;
        }

        return null;
    }

    private ModelNamespace GetOrAddNamespace(string fullNamespace)
    {
        var @namespace = _modelNamespaces.Find(x => x.FullName.Equals(fullNamespace, Ordinal));

        if (@namespace is null)
        {
            @namespace = new ModelNamespace(
                fullNamespace,
                _modelAssembly,
                GetDocumentation($"T:{fullNamespace}.{NamespaceDoc}")
            );

            _modelNamespaces.Add(@namespace);
        }

        return @namespace;
    }

    private void SetReferences()
    {
        _modelTypes.ForEach(m =>
        {
            m.NestedTypes = _modelTypes
                .Where(x => m.Id.Equals(x.ParentId, Ordinal))
                .Select(x => x.Id)
                .ToArray();
            m.DerivedTypes = _modelTypes
                .Where(x =>
                    x.BaseTypes.Union(x.Implementations, StringComparer.Ordinal)
                        .Any(t => t.Equals(m.Id, Ordinal))
                )
                .Select(x => x.Id)
                .ToArray();
        });
        _modelNamespaces.ForEach(ns =>
        {
            ns.Types = _modelTypes
                .Where(t => ns.Id.Equals(t.NamespaceId, Ordinal))
                .Select(x => x.Id)
                .ToArray();
        });

        _modelAssembly.Namespaces = _modelNamespaces;
        _modelAssembly.Types = _modelTypes;
    }

    private static bool ShouldNotBeDescribedAccessModifier(IMember entity) =>
        entity is IMethod method && method.IsExplicitInterfaceImplementation;

    private bool TryGetDocFromBaseClass(
        IEntity entity,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result =
            entity is ITypeDefinition typeDefinition
            && typeDefinition
                .EnumerateBaseTypeDefinitions()
                .Any(type => TryGetDocumentation(type, out resultDocumentation));
        baseDocumentation = resultDocumentation;
        return result;
    }

    private bool TryGetDocFromExplicit(
        IEntity entity,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result =
            entity is IMember member
            && member.IsExplicitInterfaceImplementation
            && member.ExplicitlyImplementedInterfaceMembers.Any(type =>
                TryGetDocumentation(type, out resultDocumentation)
            );
        baseDocumentation = resultDocumentation;
        return result;
    }

    private bool TryGetDocFromInterface(
        IEntity entity,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        XElement? resultDocumentation = null;
        var result = false;
        if (entity is IMember member)
        {
            var parentName = member.DeclaringTypeDefinition!.FullName;
            var entityId = member.GetIdString();

            result =
                member.DeclaringTypeDefinition is not null
                && member
                    .DeclaringTypeDefinition.EnumerateBaseTypeDefinitions()
                    .Any(type =>
                    {
                        if (type.Kind != TypeKind.Interface)
                        {
                            return false;
                        }

                        _ = TryGetDocumentationProvider(type.ParentModule!, out _);

                        var lookupId = entityId.Replace(
                            parentName,
                            type.FullName
#if !NETSTANDARD2_0
                            ,
                            OrdinalIgnoreCase
#endif
                        );
                        resultDocumentation = GetDocumentation(lookupId);
                        return resultDocumentation is not null;
                    });
        }

        baseDocumentation = resultDocumentation;
        return result;
    }

    private bool TryGetDocFromReference(
        string? reference,
        [NotNullWhen(true)] out XElement? baseDocumentation
    )
    {
        baseDocumentation = null;

        if (reference is not null)
        {
            baseDocumentation = GetDocumentation(reference);
        }

        return baseDocumentation is not null;
    }

    private bool TryGetDocumentation(
        IEntity? entity,
        [NotNullWhen(true)] out XElement? documentation
    )
    {
        documentation = null;

        if (entity is null || entity.ParentModule is null)
        {
            return false;
        }

        if (!TryGetDocumentationProvider(entity.ParentModule, out var documentationProvider))
        {
            return false;
        }

        var result = ConvertToDocumentation(documentationProvider.GetDocumentation(entity));

        if (result is null)
        {
            return false;
        }

        if (result.HasInheritDoc(out var inheritedDocumentation))
        {
            var reference = inheritedDocumentation.GetCRefAttribute();
            inheritedDocumentation.Remove();

            if (
                TryGetDocFromReference(reference, out var baseDocumentation)
                || TryGetDocFromExplicit(entity, out baseDocumentation)
                || TryGetDocFromBaseClass(entity, out baseDocumentation)
                || TryGetDocFromInterface(entity, out baseDocumentation)
            )
            {
                result = baseDocumentation.Merge(result, _ignoredElements);
            }
        }

        documentation = result;

        return documentation is not null;
    }
}
