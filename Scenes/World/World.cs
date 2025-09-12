using System.Collections.Generic;
using System.Linq;
using Fantoria.Scenes.World.Data;
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
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; private set; }
    [Export] [NotNull] public PackedScene WorldMultiplayerSpawnerPackedScene { get; private set; }

    public WorldPersistenceData Data { get; private set; }
    public readonly WorldEvents Events = new();
    
    [Export] public Godot.Collections.Dictionary<int, string> PlayerNickByPeerId = new();

    //TODO Может быть заменить ручную настройку ноды синхронизатора, на автоматическую, через пометку аннотацией типа [Sync]? Sync может быть наследником [Export], тогда, возможно, хватит только Sync.
    //TODO Возможно ли вообще избавиться от этого "Досинкхронизирования" через переменные? Все равно же должен быть какой-то ивент, чтобы Hud и т.п. узнали об изменение свойства. В World ивенты, которые подписаны на MpSpawn.Spawned ?
    public MapSurface MapSurface => GetNodeOrNull<MapSurface>(_mapSurfaceName);
    [Export] private string _mapSurfaceName;
    
    public List<BattleSurface> BattleSurfaces => _battleSurfacesNames.Select(name => GetNodeOrNull<BattleSurface>(name)).ToList();
    [Export] private Array<string> _battleSurfacesNames = new();
    
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

    private WorldMultiplayerSpawner AddSpawnerToNode(Node node)
    {
        WorldMultiplayerSpawner worldMultiplayerSpawner = WorldMultiplayerSpawnerPackedScene.Instantiate<WorldMultiplayerSpawner>()
            .Init(node);
        this.AddChildWithUniqueName(worldMultiplayerSpawner, "MultiplayerSpawner");
        node.TreeExiting += worldMultiplayerSpawner.QueueFree;
        return worldMultiplayerSpawner;
    }

    public void AddPersistenceData(byte[] serializableData = null)
    {
        Data?.QueueFree();
        if (serializableData == null)
        {
            Data = new();
        }
        else
        {
            //TODO Data = MessagePack.Deserialize
        }
        this.AddChildWithName(Data, "Data");
    }

    public byte[] GetPersistenceData()
    {
        return null; //TODO MessagePack.Serialize
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