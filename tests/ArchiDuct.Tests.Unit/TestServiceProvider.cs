namespace ArchiDuct.Tests.Unit;

using System;
using System.Collections.Generic;

internal sealed class TestServiceProvider : IServiceProvider
{
    private readonly IReadOnlyDictionary<Type, object> _services;

    public TestServiceProvider()
        : this(new Dictionary<Type, object>()) { }

    public TestServiceProvider(IReadOnlyDictionary<Type, object> services) => _services = services;

    public object? GetService(Type serviceType) => _services.TryGetValue(serviceType, out var service) ? service : null;
}
