using System;
using Fantoria.Lib.Nodes.MpSync;
using Fantoria.Scenes.World.Data;
using Fantoria.Scenes.World.Data.MapPoint;
using Fantoria.Scenes.World.Services;
using Fantoria.Scenes.World.Tree;
using Fantoria.Scenes.World.Tree.Entity.Building;
using Godot;
using WorldStartStopService = Fantoria.Scenes.World.Services.StartStop.WorldStartStopService;

namespace Fantoria.Scenes.World;

public partial class World : Node2D
{
    
    [Export] [NotNull] public WorldTree Tree { get; private set; }
    [Export] [NotNull] public WorldPersistenceData Data { get; private set; }
    [Export] [NotNull] public WorldTemporaryDataService TemporaryDataService { get; private set; }
    [Export] [NotNull] public WorldStateCheckerService StateCheckerService  { get; private set; }
    [Export] [NotNull] public WorldStartStopService StartStopService  { get; private set; }
    [Export] [NotNull] public WorldMultiplayerSpawnerService MultiplayerSpawnerService { get; private set; }
    
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; private set; }
    
    public readonly WorldEvents Events = new();
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
        
        StartStopService.Init(this);
        StateCheckerService.Init(Tree, Data);
        Tree.Init(this);
        
        this.AddChildWithName(new AttributeMultiplayerSynchronizer(this), "MultiplayerSynchronizer");
    }
    
    //TODO Test methods. Remove after tests.
    public void Test1() => RpcId(ServerId, MethodName.Test1Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test1Rpc()
    {
        Log.Warning("Test 1 RPC called");

        //TODO Первые способ создать MapPoint
        MapPoint mp1 = MapPoint.Create(PackedScenes.MapPoint, Data.MapPoint, data =>
        {
            data.PositionX = Random.Shared.Next(0, 600);
            data.PositionY = Random.Shared.Next(0, 600);
        });
        this.AddChildWithUniqueName(mp1, "MapPoint");
        
        //TODO Второй способ создать MapPoint
        MapPointData mapPointData = new MapPointData();
        mapPointData.PositionX = Random.Shared.Next(0, 600);
        mapPointData.PositionY = Random.Shared.Next(0, 600);
        
        Data.MapPoint.AddMapPoint(mapPointData);
        MapPoint mp2 = PackedScenes.MapPoint.Instantiate<MapPoint>().Init(mapPointData);
        this.AddChildWithUniqueName(mp2, "MapPoint");
        
        //TODO Третий способ создать MapPoint: отдельный объект WorldFactories, где лежат фабрики под все персистенсе объекты 
    }
    
    public void Test2() => RpcId(ServerId, MethodName.Test2Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test2Rpc()
    {
        Log.Warning("Test 2 RPC called");
    }
    
    public void Test3() => RpcId(ServerId, MethodName.Test3Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test3Rpc()
    {
        Log.Warning("Test 3 RPC called");
    }
}