using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fantoria.Lib.Services.NotNullChecking;

[Service]
public class NotNullCheckerService
{

    private readonly HashSet<Type> _checked = new();
    
    public void CheckProperties(object obj)
    {
        if (_checked.Contains(obj.GetType())) return;
        CheckPropertiesForce(obj);
    }
    
    public void CheckPropertiesForce(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        Type type = obj.GetType();
        foreach (PropertyInfo property in type.GetProperties())
        {
            if (Attribute.IsDefined(property, typeof(NotNullAttribute)) && // If it has NotNull attribute
                property.GetValue(obj) == null) // And it is null
            {
                Log.Critical($"Property '{property.Name}' in type '{obj.GetType()}' is null, but has NotNull attribute");
            }
        }

        _checked.Add(type);
    }
}
