using System.Collections.Generic;
using System.Linq;
using Fantoria.Lib.Nodes.MpSync;
using Fantoria.Scenes.World.Data;
using Fantoria.Scenes.World.Services;
using Fantoria.Scenes.World.Tree;
using Godot;
using Godot.Collections;
using MapPoint = Fantoria.Scenes.World.Tree.Entity.Building.MapPoint;

namespace Fantoria.Scenes.World;

public partial class World : Node2D
{
    
    //TODO WorldPackedScenes переименовать в SyncPackedScenes/ServerPackedScenes? И отдельно сделать ClientPackedScenes, который не привязан в MpSpawner. В нем всякие эффекты (не гуи, а мировые).
    
    [Export] [NotNull] public WorldTree Tree { get; private set; }
    [Export] [NotNull] public WorldPersistenceData Data { get; private set; }
    [Export] [NotNull] public WorldStateChecker StateChecker  { get; private set; }
    [Export] [NotNull] public WorldMultiplayerSpawnerService MultiplayerSpawnerService { get; private set; }
    
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; private set; }
    
    public readonly WorldEvents Events = new();
    public WorldStartStop StartStop;
    
    
    /// <summary>
    /// Hoster nick, or nick from cmd param in dedicated server
    /// Player.IsAdmin in WorldPersistenceData for this player automatically will change to true
    /// If next application start will be with MainAdminNick = null, then Player.IsAdmin in WorldPersistenceData stay true anyway
    /// </summary>
    [Export] [Sync] public string MainAdminNick; 
    
    /// <summary>
    /// List of current connected players
    /// </summary>
    [Export]  [Sync] public Godot.Collections.Dictionary<int, string> PlayerNickByPeerId = new();
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
        
        StartStop = new WorldStartStop(this);
        StateChecker.Init(Tree, Data);
        
        Tree.Init(this);
        
        this.AddChildWithName(new AttributeMultiplayerSynchronizer(this), "MultiplayerSynchronizer");
    }
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree && this.IsServer()) StartStop.Shutdown();
    }
    
    //TODO Test methods. Remove after tests.
    public void Test1() => RpcId(ServerId, MethodName.Test1Rpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void Test1Rpc()
    {
        Log.Warning("Test 1 RPC called");
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