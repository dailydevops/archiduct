namespace NetEvolve.ArchiDuct.Models;

using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.FluentValue;

internal sealed record SourceFilter(Func<ITypeDefinition, object?> ValueSelector, IConstraint Constraint)
{
    public bool IsSatisfiedBy(ITypeDefinition typeDefinition) =>
        Constraint.IsSatisfiedBy(ValueSelector(typeDefinition));
};
