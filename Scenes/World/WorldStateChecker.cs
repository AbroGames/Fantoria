using System.Text;
using Fantoria.Lib.Utils.Cooldown;
using Godot;

namespace Fantoria.Scenes.World;

public partial class WorldStateChecker : Node
{

    private World _world;
    private AutoCooldown _checkCooldown;

    public void Init(World world)
    {
        _world = world;
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
    
    public void StateCheckOnClients() => StateCheckOnClients(_world.GetTreeHash(), _world.Data.GetDataHash());
    private void StateCheckOnClients(string serverWorldTreeHash, string serverWorldDataHash) => 
        Rpc(MethodName.StateCheckOnClientRpc, serverWorldTreeHash, serverWorldDataHash);
    [Rpc(CallLocal = false)]
    private void StateCheckOnClientRpc(string serverWorldTreeHash, string serverWorldDataHash)
    {
        if (_world.GetTreeHash() != serverWorldTreeHash || _world.Data.GetDataHash() != serverWorldDataHash)
        {
            NotifyServerAboutInconsistentState(_world.GetFullTree(), _world.Data.GetFullData());
        }
    }

    private void NotifyServerAboutInconsistentState(string clientWorldTree, string clientWorldData) => 
        RpcId(ServerId, MethodName.NotifyServerAboutInconsistentStateRpc, clientWorldTree, clientWorldData);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false)]
    private void NotifyServerAboutInconsistentStateRpc(string clientWorldTree, string clientWorldData)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Client has inconsistent state (peer id = {GetMultiplayer().GetRemoteSenderId()})");

        if (!_world.GetFullTree().Equals(clientWorldTree))
        {
            sb.AppendLine("Server world tree: " + _world.GetFullTree());
            sb.AppendLine("Client world tree: " + clientWorldTree);
        }
        
        if (!_world.Data.GetFullData().Equals(clientWorldData))
        {
            sb.AppendLine("Server world data: " + _world.Data.GetFullData());
            sb.AppendLine("Client world data: " + clientWorldData);
        }
        
        Log.Warning(sb.ToString());
    }
    
    public void LogState() => Rpc(MethodName.LogStateRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void LogStateRpc()
    {
        Log.Debug("World tree: " + _world.GetFullTree());
        Log.Debug("World data: " + _world.Data.GetFullData());
        Log.Debug("World tree hash: " + _world.GetTreeHash());
        Log.Debug("World data hash: " + _world.Data.GetDataHash());
    }
}