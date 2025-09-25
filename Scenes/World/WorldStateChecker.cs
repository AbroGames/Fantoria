using System.Text;
using Fantoria.Lib.Utils.Cooldown;
using Fantoria.Scenes.World.Data;
using Fantoria.Scenes.World.Tree;
using Godot;

namespace Fantoria.Scenes.World;

public partial class WorldStateChecker : Node
{

    private WorldTree _worldTree;
    private WorldPersistenceData _worldData;
    private AutoCooldown _checkCooldown;

    public void Init(WorldTree worldTree, WorldPersistenceData worldData)
    {
        _worldTree = worldTree;
        _worldData = worldData;
    }

    public void InitOnServer()
    {
        _checkCooldown = new AutoCooldown(5, true, StateCheckOnClients);
    }

    public override void _PhysicsProcess(double delta)
    {
        _checkCooldown?.Update(delta);
    }

    public void StateCheckRequest() => RpcId(ServerId, MethodName.StateCheckRequestRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void StateCheckRequestRpc()
    {
        StateCheckOnClients();
    }
    
    public void StateCheckOnClients() => StateCheckOnClients(_worldTree.GetTreeHash(), _worldData.GetDataHash());
    private void StateCheckOnClients(string serverWorldTreeHash, string serverWorldDataHash) => 
        Rpc(MethodName.StateCheckOnClientRpc, serverWorldTreeHash, serverWorldDataHash);
    [Rpc(CallLocal = false)]
    private void StateCheckOnClientRpc(string serverWorldTreeHash, string serverWorldDataHash)
    {
        if (_worldTree.GetTreeHash() != serverWorldTreeHash || _worldData.GetDataHash() != serverWorldDataHash)
        {
            NotifyServerAboutInconsistentState(_worldTree.GetFullTree(), _worldData.GetFullData());
        }
    }

    private void NotifyServerAboutInconsistentState(string clientWorldTree, string clientWorldData) => 
        RpcId(ServerId, MethodName.NotifyServerAboutInconsistentStateRpc, clientWorldTree, clientWorldData);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    private void NotifyServerAboutInconsistentStateRpc(string clientWorldTree, string clientWorldData)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Client has inconsistent state (peer id = {GetMultiplayer().GetRemoteSenderId()})");

        if (!_worldTree.GetFullTree().Equals(clientWorldTree))
        {
            sb.AppendLine("Server world tree: " + _worldTree.GetFullTree());
            sb.AppendLine("Client world tree: " + clientWorldTree);
        }
        
        if (!_worldData.GetFullData().Equals(clientWorldData))
        {
            sb.AppendLine("Server world data: " + _worldData.GetFullData());
            sb.AppendLine("Client world data: " + clientWorldData);
        }
        
        Log.Warning(sb.ToString());
    }
    
    /// <summary>
    /// Log full Tree and Data infos.
    /// For Debug only.
    /// </summary>
    public void LogState() => Rpc(MethodName.LogStateRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void LogStateRpc()
    {
        Log.Debug("World tree: " + _worldTree.GetFullTree());
        Log.Debug("World data: " + _worldData.GetFullData());
        Log.Debug("World tree hash: " + _worldTree.GetTreeHash());
        Log.Debug("World data hash: " + _worldData.GetDataHash());
    }
}