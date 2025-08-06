using System;

namespace Fantoria.Lib.Services;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute(string name = null, int order = 0) : Attribute
{
    public string Name { get; private set; } = name;
    public int OrderInitialization { get; private set; } = order;
}