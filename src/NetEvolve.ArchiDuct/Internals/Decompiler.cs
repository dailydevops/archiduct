﻿namespace NetEvolve.ArchiDuct.Internals;

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Resolver;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models;
using NetEvolve.ArchiDuct.Models.Abstractions;
using static NetEvolve.ArchiDuct.Internals.Constants;

internal sealed partial class Decompiler
{
    private readonly CSharpDecompiler _decompiler;
    private readonly CSharpResolver _resolver;

    private readonly Version _zeroVersion = Version.Parse("0.0.0.0");

    public Decompiler(string assemblyFile, DecompilerSettings decompilerSettings)
    {
        _decompiler = new CSharpDecompiler(assemblyFile, decompilerSettings);
        _resolver = new CSharpResolver(_decompiler.TypeSystem);
    }

    public List<ModelAssembly> Decompile(HashSet<SourceFilter> filters, bool includeReferences = false)
    {
        var typeSystem = _decompiler.TypeSystem;

        var result = new List<ModelAssembly>();

        _ = Parallel.ForEach(
            typeSystem.Modules,
            module =>
            {
                _ = ModelFactory.TryGetDocumentationProvider(module, out var _);
                if (!module.IsMainModule && !includeReferences)
                {
                    return;
                }

                var model = DecompileModule(module, filters);

                model.References =
                [
                    .. typeSystem
                        .ReferencedModules.Where(module => module.AssemblyVersion != _zeroVersion)
                        .Select(module => new ModelReference(module)),
                ];

                result.Add(model);
            }
        );

        return result;
    }

    private ModelAssembly DecompileModule(IModule module, HashSet<SourceFilter> filters)
    {
        var modelAssembly = new ModelAssembly(
            module,
            ModelFactory.GetDocumentation($"T:{module.AssemblyName}.{AssemblyDoc}", _resolver)
        );

        MapAttributes(modelAssembly, module);
        MapTypeModels(modelAssembly, module, filters);
        SetReferences(modelAssembly);

        return modelAssembly;
    }

    private void MapAttributes(ModelAssembly modelAssembly, IModule module)
    {
        foreach (var attribute in module.GetAssemblyAttributes())
        {
            if (attribute is null)
            {
                continue;
            }

            var typeDefinition = attribute.AttributeType.GetDefinition()!;

            if (typeDefinition is null)
            {
                continue;
            }

            _ = modelAssembly.Attributes.Add(
                new ModelAttribute(
                    attribute,
                    typeDefinition,
                    ModelFactory.GetDocumentation(typeDefinition.GetIdString(), _resolver)
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

    private static void MapModelMemberParameters(ModelMemberBase result, IMember member)
    {
        if (result is ModelMemberAdvancedBase modelWithParameters)
        {
            if (member is IMethod genericMethod && genericMethod.TypeParameters.Any())
            {
                modelWithParameters.TypeParameters =
                [
                    .. genericMethod.TypeParameters.Select(p => new ModelTypeParameter(p, modelWithParameters)),
                ];
            }

            if (member is IParameterizedMember parameterizedMember && parameterizedMember.Parameters.Any())
            {
                modelWithParameters.Parameters =
                [
                    .. parameterizedMember.Parameters.Select(p => new ModelParameter(p, modelWithParameters)),
                ];
            }
        }
        else if (
            result is ModelIndexer modelIndexer
            && member is IParameterizedMember parameterized
            && parameterized.Parameters.Any()
        )
        {
            modelIndexer.Parameters = [.. parameterized.Parameters.Select(p => new ModelParameter(p, modelIndexer))];
        }
    }

    private static void MapModelTypeParameters(ITypeDefinition typeDefinition, ModelTypeBase model)
    {
        if (typeDefinition.TypeParameters.Any())
        {
            model.TypeParameters = [.. typeDefinition.TypeParameters.Select(p => new ModelTypeParameter(p, model))];
        }
    }

    private void MapMemberModel(ModelAssembly modelAssembly, IMember member, ModelTypeBase parent)
    {
        if (member.IsCompilerGeneratedOrIsInCompilerGeneratedClass())
        {
            return;
        }

        if (!ModelFactory.TryGetDocumentation(member, _resolver, out var doc))
        {
            //Include undocumented items?
        }

        if (parent is ModelEnum && member is IField enumField && enumField.Name.Equals("value__", Ordinal))
        {
            return;
        }

        var modelMember = ModelFactory.CreateModelMemberType(member, parent, doc, _resolver);
        MapModelMemberParameters(modelMember, member);
        //Map TypeParameters for methods and indexers
        //ReturnAttributes

        _ = modelAssembly.Members.Add(modelMember);
        _ = parent.Members.Add(modelMember.Id);
    }

    private void MapMemberModels(
        ModelAssembly modelAssembly,
        ITypeDefinition typeDefinition,
        ModelTypeBase describedModel
    )
    {
        if (typeDefinition.Kind == TypeKind.Delegate)
        {
            return;
        }

        foreach (var member in typeDefinition.Members)
        {
            MapMemberModel(modelAssembly, member, describedModel);
        }
    }

    private void MapTypeModel(
        ModelAssembly modelAssembly,
        ITypeDefinition typeDefinition,
        ModelEntityBase? parent = null
    )
    {
        if (IsTypeDefinitionExcluded(typeDefinition))
        {
            return;
        }

        if (ModelFactory.TryGetDocumentation(typeDefinition, _resolver, out var doc))
        {
            //Include undocumented items?
        }

        if (parent is null)
        {
            parent = GetOrAddNamespace(modelAssembly, typeDefinition.Namespace);
        }

        var typeModel = ModelFactory.CreateModelType(typeDefinition, parent, doc, _resolver);

        MapModelTypeParameters(typeDefinition, typeModel);
        MapMemberModels(modelAssembly, typeDefinition, typeModel);

        _ = modelAssembly.Types.Add(typeModel);

        foreach (var nestedTypeDefinition in typeDefinition.NestedTypes)
        {
            MapTypeModel(modelAssembly, nestedTypeDefinition, typeModel);
        }
    }

    private void MapTypeModels(ModelAssembly modelAssembly, IModule module, HashSet<SourceFilter> filters)
    {
        foreach (var typeDefinition in module.TopLevelTypeDefinitions)
        {
            if (filters.Count != 0 && !filters.All(filter => filter.IsSatisfiedBy(typeDefinition)))
            {
                continue;
            }

            MapTypeModel(modelAssembly, typeDefinition);
        }
    }

    private ModelNamespace GetOrAddNamespace(ModelAssembly modelAssembly, string fullNamespace)
    {
        var @namespace = modelAssembly.Namespaces.FirstOrDefault(x => x.FullName.Equals(fullNamespace, Ordinal));

        if (@namespace is null)
        {
            @namespace = new ModelNamespace(
                fullNamespace,
                modelAssembly,
                ModelFactory.GetDocumentation($"T:{fullNamespace}.{NamespaceDoc}", _resolver)
            );

            _ = modelAssembly.Namespaces.Add(@namespace);
        }

        return @namespace;
    }

    private static void SetReferences(ModelAssembly modelAssembly)
    {
        foreach (var type in modelAssembly.Types)
        {
            type.NestedTypes = modelAssembly
                .Types.Where(x => type.Id.Equals(x.ParentId, Ordinal))
                .Select(x => x.Id)
                .ToHashSet(StringComparer.Ordinal);

            type.DerivedTypes = modelAssembly
                .Types.Where(x =>
                    x.BaseTypes.Union(x.Implementations, StringComparer.Ordinal).Any(t => t.Equals(type.Id, Ordinal))
                )
                .Select(x => x.Id)
                .ToHashSet(StringComparer.Ordinal);
        }

        foreach (var member in modelAssembly.Members)
        {
            if (member is not ModelMemberAdvancedBase overloadable)
            {
                continue;
            }

            overloadable.OverloadedMembers = modelAssembly
                .Members.Where(x =>
                    member.Kind == x.Kind
                    && !member.Id.Equals(x.Id, Ordinal)
                    && member.ParentId.Equals(x.ParentId, Ordinal)
                    && member.Name.Equals(x.Name, Ordinal)
                )
                .Select(x => x.Id)
                .ToHashSet(StringComparer.Ordinal);
        }

        foreach (var ns in modelAssembly.Namespaces)
        {
            ns.Types = modelAssembly
                .Types.Where(t => ns.Id.Equals(t.NamespaceId, Ordinal))
                .Select(x => x.Id)
                .ToHashSet(StringComparer.Ordinal);
        }
    }
}
