namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using System.ComponentModel;
using NetEvolve.ArchiDuct.Tests.Integration._internals;

[InheritsTests]
[ClassDataSource<EventAccesorsTypeProvider>(Shared = SharedType.PerClass)]
public class Decompiler_ExampleEventAccesors_Tests(EventAccesorsTypeProvider provider)
    : TestCaseBase<EventAccesorsTypeProvider>(provider) { }

public sealed class EventAccesorsTypeProvider() : TypeProviderBase(typeof(EventAccesors)) { }

public class EventAccesors : INotifyPropertyChanged
{
    private volatile string? _event;

    public event PropertyChangedEventHandler? PropertyChanged;

#pragma warning disable RCS1159 // Use EventHandler<T>
    internal event PropertyChangedEventHandler OnChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }
#pragma warning restore RCS1159 // Use EventHandler<T>

    public string? Event
    {
        get => _event;
        set
        {
            _event = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Event)));
        }
    }
}
