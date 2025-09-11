using System.Collections.Generic;
using System.Linq;
using Fantoria.Scenes.World.Data.PlayersData;
using Fantoria.Scenes.World.Entity.Building;
using Fantoria.Scenes.World.StartStop;
using Fantoria.Scenes.World.Surface.Battle;
using Fantoria.Scenes.World.Surface.Map;
using Godot;
using Godot.Collections;

namespace Fantoria.Scenes.World;

public partial class World : Node2D
{
    
    //TODO WorldPackedScenes переименовать в SyncPackedScenes? И отдельно сделать ClientPackedScenes, который не привязан в MpSpawner. В нем всякие эффекты (не гуи, а мировые).
    //TODO А может лучше всё-таки оставить World ПОЛНОСТЬЮ синхронным, а World-эффекты вешать в отдельный слой, по соседству с World. Принцип как у HUD.
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; set; }
    [Export] [NotNull] public PackedScene WorldMultiplayerSpawnerPackedScene { get; private set; }

    public readonly WorldEvents Events = new();
    
    [Export] public string SaveFilePath; // World will be to save to this file on server

    //TODO Может быть заменить ручную настройку ноды синхронизатора, на автоматическую, через пометку аннотацией типа [Sync]? Sync может быть наследником [Export], тогда, возможно, хватит только Sync.
    //TODO Возможно ли вообще избавиться от этого "Досинкхронизирования" через переменные? Все равно же должен быть какой-то ивент, чтобы Hud и т.п. узнали об изменение свойства. В World ивенты, которые подписаны на MpSpawn.Spawned ?
    private MapSurface MapSurface => GetNodeOrNull<MapSurface>(_mapSurfaceName);
    [Export] private string _mapSurfaceName;
    
    private List<BattleSurface> BattleSurfaces => _battleSurfacesNames.Select(name => GetNode<BattleSurface>(name)).ToList();
    [Export] private Array<string> _battleSurfacesNames = new();
    
    public PlayersData PlayersData => GetNodeOrNull<PlayersData>(_playersDataName);
    [Export] private string _playersDataName;
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }

    public MapSurface AddMapSurface()
    {
        MapSurface?.QueueFree();
        MapSurface mapSurface = PackedScenes.MapSurface.Instantiate<MapSurface>();
        this.AddChildWithUniqueName(mapSurface, "MapSurface");
        _mapSurfaceName = mapSurface.Name;
        AddSpawnerToNode(mapSurface);
        return mapSurface;
    }
    
    public BattleSurface AddBattleSurface()
    {
        BattleSurface battleSurface = PackedScenes.BattleSurface.Instantiate<BattleSurface>();
        this.AddChildWithUniqueName(battleSurface, "BattleSurface");
        _battleSurfacesNames.Add(battleSurface.Name);
        AddSpawnerToNode(battleSurface);
        return battleSurface;
    }

    public PlayersData AddPlayersData()
    {
        PlayersData?.QueueFree();
        PlayersData playersData = PackedScenes.PlayersData.Instantiate<PlayersData>();
        this.AddChildWithUniqueName(playersData, "PlayersData");
        _playersDataName = playersData.Name;
        AddSpawnerToNode(playersData);
        return playersData;
    }

    private void AddSpawnerToNode(Node node)
    {
        WorldMultiplayerSpawner worldMultiplayerSpawner = WorldMultiplayerSpawnerPackedScene.Instantiate<WorldMultiplayerSpawner>()
            .Init(node);
        this.AddChildWithUniqueName(worldMultiplayerSpawner, "MultiplayerSpawner");
        node.TreeExiting += worldMultiplayerSpawner.QueueFree;
    }

    public void CreatePoint() => RpcId(ServerId, MethodName.CreatePointRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void CreatePointRpc()
    {
        Log.Warning("CreatePoint RPC called");
    }
    
    public void LogTree() => Rpc(MethodName.LogTreeRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void LogTreeRpc()
    {
        Log.Debug(this.GetFullTree());
        Log.Debug(this.GetTreeHash());
    }
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree && this.IsServer()) new WorldStopper(this).StopOnServer();
    }
}