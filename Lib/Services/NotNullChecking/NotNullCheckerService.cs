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
            bool isNull = property.GetValue(obj) == null;
            bool hasNotNullAttribute = Attribute.IsDefined(property, typeof(NotNullAttribute));
            if (hasNotNullAttribute && isNull)
            {
                Log.Critical($"Property '{property.Name}' in type '{obj.GetType()}' is null, but has NotNull attribute");
            }
        }

        _checked.Add(type);
    }
}
