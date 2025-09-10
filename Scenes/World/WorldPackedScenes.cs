using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

namespace Fantoria.Scenes.World;

public partial class WorldPackedScenes : Node
{
    
    [ExportGroup("Surfaces")]
    [Export] [NotNull] public PackedScene MapSurface { get; private set; }
    [Export] [NotNull] public PackedScene BattleSurface { get; private set; }
    
    [ExportGroup("Map")]
    [Export] [NotNull] public PackedScene MapPoint { get; private set; }
    
    [ExportGroup("Data")]
    [Export] [NotNull] public PackedScene PlayersData { get; private set; }

    public Godot.Collections.Dictionary<string, PackedScene> SceneByName { get; private set; }
    public ICollection<PackedScene> Scenes => SceneByName.Values;
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
        SceneByName = ParseScenes();
    }
    
    private Godot.Collections.Dictionary<string, PackedScene> ParseScenes()
    {
        Godot.Collections.Dictionary<string, PackedScene> scenes = new();
        
        Type type = GetType();
        foreach (PropertyInfo property in type.GetProperties())
        {
            if (!property.PropertyType.IsAssignableTo(typeof(PackedScene))) continue;
            if (!Attribute.IsDefined(property, typeof(ExportAttribute))) continue;

            scenes[property.Name] = property.GetValue(this) as PackedScene;
        }
        
        return scenes;
    }
}