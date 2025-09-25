using System.Collections.Generic;
using System.Linq;
using Fantoria.Lib.Nodes.MpSync;
using Godot;
using Godot.Collections;
using BattleSurface = Fantoria.Scenes.World.Tree.Surface.Battle.BattleSurface;
using MapSurface = Fantoria.Scenes.World.Tree.Surface.Map.MapSurface;

namespace Fantoria.Scenes.World.Tree;

public partial class WorldTree : Node2D
{

    private World _world;
    
    //TODO Возможно ли вообще избавиться от этого "Досинхронизирования" через переменные? Все равно же должен быть какой-то ивент, чтобы Hud и т.п. узнали об изменение свойства. Или сделать в World ивенты, которые подписаны на MpSpawn.Spawned ?
    public MapSurface MapSurface => GetNodeOrNull<MapSurface>(_mapSurfaceName);
    [Export] [Sync] private string _mapSurfaceName;
    
    public List<BattleSurface> BattleSurfaces => _battleSurfacesNames.Select(name => GetNodeOrNull<BattleSurface>(name)).ToList();
    [Export] [Sync] private Array<string> _battleSurfacesNames = new();

    public void Init(World world)
    {
        _world = world;
    }

    public override void _Ready()
    {
        this.AddChildWithName(new AttributeMultiplayerSynchronizer(this), "MultiplayerSynchronizer");
    }

    public MapSurface AddMapSurface()
    {
        MapSurface?.QueueFree();
        MapSurface mapSurface = _world.PackedScenes.MapSurface.Instantiate<MapSurface>();
        this.AddChildWithUniqueName(mapSurface, "MapSurface");
        _mapSurfaceName = mapSurface.Name;
        _world.MultiplayerSpawnerService.AddSpawnerToNode(mapSurface);
        return mapSurface;
    }
    
    public BattleSurface AddBattleSurface()
    {
        BattleSurface battleSurface = _world.PackedScenes.BattleSurface.Instantiate<BattleSurface>();
        this.AddChildWithUniqueName(battleSurface, "BattleSurface");
        _battleSurfacesNames.Add(battleSurface.Name);
        _world.MultiplayerSpawnerService.AddSpawnerToNode(battleSurface);
        return battleSurface;
    }
}