namespace NetEvolve.ArchiDuct.Tests.Integration._internals;

public sealed class GenericTypeProvider<TType> : TypeProviderBase
    where TType : notnull
{
    public GenericTypeProvider()
        : base(typeof(TType)) { }
}
