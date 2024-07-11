namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.ComponentModel;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_ExampleEventAccesors_Tests(EventAccesorsTypeProvider provider)
    : TestCaseBase<EventAccesorsTypeProvider>(provider) { }

public sealed class EventAccesorsTypeProvider() : TypeProviderBase(typeof(EventAccesors)) { }

public class EventAccesors : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    internal event PropertyChangedEventHandler OnChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }
}
