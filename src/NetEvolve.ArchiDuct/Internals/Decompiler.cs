namespace NetEvolve.ArchiDuct.Internals;

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
    private readonly CSharpDecompiler _decompiler;

    private readonly List<ModelNamespace> _modelNamespaces;

    private readonly List<ModelTypeBase> _modelTypes;

    private readonly CSharpResolver _resolver;

    private readonly Version _zeroVersion = Version.Parse("0.0.0.0");

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

        if (ModelFactory.TryGetDocumentationProvider(mainModule, out var documentationProvider))
        {
            _decompiler.DocumentationProvider = documentationProvider;
        }

        var model = DecompileModule(mainModule, filters);

        model.References = typeSystem
            .ReferencedModules.Where(module => module.AssemblyVersion != _zeroVersion)
            .Select(module => new ModelReference(module))
            .ToHashSet();

        return model;
    }

    private ModelAssembly DecompileModule(IModule module, HashSet<SourceFilter> filters)
    {
        _modelAssembly = new ModelAssembly(
            module,
            ModelFactory.GetDocumentation($"T:{module.AssemblyName}.{AssemblyDoc}", _resolver)
        );

        MapAttributes(module);
        MapTypeModels(module, filters);
        SetReferences();

        // TODO: Add GitVersion informations

        return _modelAssembly;
    }

    private void MapAttributes(IModule module)
    {
        var attributes = module.GetAssemblyAttributes();

        foreach (var attribute in attributes)
        {
            if (attribute is null)
            {
                continue;
            }

            var typeDefinition = attribute.AttributeType.GetDefinition()!;

            _ = _modelAssembly.Attributes.Add(
                new ModelAttribute(
                    attribute,
                    typeDefinition,
                    ModelFactory.GetDocumentation(typeDefinition?.GetIdString(), _resolver)
                )
            );
        }
    }

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
                .ToHashSet();
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
                    .ToHashSet();
            }

            if (
                member is IParameterizedMember parameterizedMember
                && parameterizedMember.Parameters.Any()
            )
            {
                modelWithParameters.Parameters = parameterizedMember
                    .Parameters.Select(p => new ModelParameter(p, modelWithParameters))
                    .ToHashSet();
            }
        }
        else if (
            result is ModelIndexer modelIndexer
            && member is IParameterizedMember parameterized
            && parameterized.Parameters.Any()
        )
        {
            modelIndexer.Parameters = parameterized
                .Parameters.Select(p => new ModelParameter(p, modelIndexer))
                .ToHashSet();
        }
    }

    private static void MapModelTypeParameters(ITypeDefinition typeDefinition, ModelTypeBase model)
    {
        if (typeDefinition.TypeParameters.Any())
        {
            model.TypeParameters = typeDefinition
                .TypeParameters.Select(p => new ModelTypeParameter(p, model))
                .ToHashSet();
        }
    }

    private void MapMemberModel(IMember member, ModelTypeBase parentModel)
    {
        if (member.IsCompilerGeneratedOrIsInCompilerGeneratedClass())
        {
            return;
        }

        if (!ModelFactory.TryGetDocumentation(member, _resolver, out var documentation))
        {
            //TODO: Include undocumented items?
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
            documentation,
            _resolver
        );
        MapModelMemberParameters(member, describedMember);
        //TODO: Map TypeParameters for methods and indexers
        //TODO: ReturnAttributes

        parentModel.AddMember(describedMember);
    }

    private void MapMemberModels(ITypeDefinition typeDefinition, ModelTypeBase describedModel)
    {
        if (typeDefinition.Kind == TypeKind.Delegate)
        {
            return;
        }

        foreach (var member in typeDefinition.Members)
        {
            MapMemberModel(member, describedModel);
        }

        MapModelMemberOverloads(describedModel);
    }

    private void MapTypeModel(ITypeDefinition typeDefinition, ModelEntityBase? parentEntity = null)
    {
        if (IsTypeDefinitionExcluded(typeDefinition))
        {
            return;
        }

        if (ModelFactory.TryGetDocumentation(typeDefinition, _resolver, out var documentation))
        {
            //TODO: Include undocumented items?
        }

        var describedModel = ModelFactory.CreateModelType(
            typeDefinition,
            parentEntity ?? GetOrAddNamespace(typeDefinition.Namespace),
            documentation,
            _resolver
        );

        MapModelTypeParameters(typeDefinition, describedModel);
        MapMemberModels(typeDefinition, describedModel);

        _modelTypes.Add(describedModel);

        foreach (var nestedTypeDefinition in typeDefinition.NestedTypes)
        {
            MapTypeModel(nestedTypeDefinition, describedModel);
        }
    }

    private void MapTypeModels(IModule module, HashSet<SourceFilter> filters)
    {
        foreach (var typeDefinition in module.TopLevelTypeDefinitions)
        {
            if (filters.Count != 0 && !filters.All(filter => ApplyFilter(filter, typeDefinition)))
            {
                continue;
            }

            MapTypeModel(typeDefinition);
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

    private ModelNamespace GetOrAddNamespace(string fullNamespace)
    {
        var @namespace = _modelNamespaces.Find(x => x.FullName.Equals(fullNamespace, Ordinal));

        if (@namespace is null)
        {
            @namespace = new ModelNamespace(
                fullNamespace,
                _modelAssembly,
                ModelFactory.GetDocumentation($"T:{fullNamespace}.{NamespaceDoc}", _resolver)
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
                .ToHashSet();
            m.DerivedTypes = _modelTypes
                .Where(x =>
                    x.BaseTypes.Union(x.Implementations, StringComparer.Ordinal)
                        .Any(t => t.Equals(m.Id, Ordinal))
                )
                .Select(x => x.Id)
                .ToHashSet();
        });
        _modelNamespaces.ForEach(ns =>
        {
            ns.Types = _modelTypes
                .Where(t => ns.Id.Equals(t.NamespaceId, Ordinal))
                .Select(x => x.Id)
                .ToHashSet();
        });

        _modelAssembly.Namespaces = [.. _modelNamespaces];
        _modelAssembly.Types = [.. _modelTypes];
    }
}
