using System;

namespace Fantoria.Lib.Services.NotNullChecking;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotNullAttribute : Attribute
{
}
