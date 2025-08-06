using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Fantoria.Lib.Services;

public static class ServiceLocator
{
    private static Dictionary<string, object> _serviceByName = new();
    private static Dictionary<Type, object> _serviceByType = new();

    public static void Init()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Select(t => new
            {
                Type = t,
                Attr = t.GetCustomAttribute<ServiceAttribute>()
            })
            .Where(x => x.Attr != null)
            .OrderBy(x => x.Attr.OrderInitialization);
        
        foreach (var entry in types) {
            var type = entry.Type;
            var attr = entry.Attr;
            var name = attr.Name ?? type.Name;

            var instance = Activator.CreateInstance(type);

            _serviceByName[name] = instance;
            _serviceByType[type] = instance;
        }
    }

    public static object Get(string name)
    {
        if (_serviceByName.TryGetValue(name, out var instance)) return instance;

        throw new Exception($"Service by name '{name}' not found. Does it have [Service] annotation?");
    }

    public static T Get<T>()
    {
        if (_serviceByType.TryGetValue(typeof(T), out var instance)) return (T) instance;

        throw new Exception($"Service by type '{typeof(T).Name}' not found. Does it have [Service] annotation?");
    }
}