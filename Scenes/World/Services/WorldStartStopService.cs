using System.Linq;
using Godot;

namespace Fantoria.Scenes.World.Services;


public partial class WorldStartStopService : Node
{
    
    private World _world;

    /// <summary>
    /// In shutdown process (TreeExit signal) we can't use Node.GetMultiplayer().IsServer() for checking, because after TreeExit Node.GetMultiplayer() returns null.
    /// Also, Game.Network may have been disabled earlier, in which case it replaces the Peer with an OfflinePeer during the shutdown process.
    /// </summary>
    private bool _isServer;

    public WorldStartStopService Init(World world)
    {
        _world = world;
        return this;
    }
    
    public void StartNewGame(string adminNickname = null)
    {
        ServerInit(adminNickname);
        _world.Tree.AddMapSurface();
    }
    
    public void LoadGame(string saveFileName, string adminNickname = null)
    {
        ServerInit(adminNickname);
        _world.Data.SaveLoad.Load(saveFileName);
        
        //TODO Restore World state from data
    }

    private void ServerInit(string adminNickname = null)
    {
        _isServer = true;
        
        _world.TemporaryDataService.InitOnServer(adminNickname);
        _world.StateCheckerService.InitOnServer();
    }
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree && _isServer) Shutdown();
    }
    
    private void Shutdown()
    {
        _world.Data.SaveLoad.AutoSave();
    }
}