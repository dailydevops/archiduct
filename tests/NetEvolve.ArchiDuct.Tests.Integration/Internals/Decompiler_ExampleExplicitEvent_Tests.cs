namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleExplicitEvent_Tests(ExampleExplicitEventTypeProvider provider)
    : TestCaseBase<ExampleExplicitEventTypeProvider>(provider) { }

public sealed class ExampleExplicitEventTypeProvider() : TypeProviderBase(typeof(ExampleExplicitEvent)) { }

public class ExampleExplicitEvent : IExplicitEvent
{
    event EventHandler IExplicitEvent.OnChange
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
}

public interface IExplicitEvent
{
    event EventHandler OnChange;
}
