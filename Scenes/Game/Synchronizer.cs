using System;
using Godot;

namespace Fantoria.Scenes.Game;

/// <summary>
/// Use for send player data (like nick, color etc.) to server.
/// We must use synchronizer out of World, because in connecting process World children nodes don't exist.
/// </summary>
public partial class Synchronizer : Node
{

    public Action SyncStartedOnClientEvent;
    public Action SyncEndedOnClientEvent;
    public Action<string> SyncRejectOnClientEvent;
    
    private World.World _world;
    
    public Synchronizer Init(World.World world)
    {
        if (world == null) Log.Error("World must be not null");
        _world = world;

        return this;
    }

    public void StartSyncOnClient()
    {
        SyncStartedOnClientEvent.Invoke();
        string nick = "TestNick-" + Random.Shared.Next(); //TODO Take player info from Game/Settings
        NewClientInitOnServer(nick);
    }

    private void NewClientInitOnServer(string nick) => RpcId(ServerId, MethodName.NewClientInitOnServerRpc, nick);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)] 
    private void NewClientInitOnServerRpc(string nick)
    {
        //TODO Receive and store nickname, color and etc in World.PlayerData. Client info automatically resends to another clients by MpSync.
        int connectedClientId = GetMultiplayer().GetRemoteSenderId();

        if (_world.PlayerNickByPeerId.Values.Contains(nick))
        {
            RejectSyncOnClient(connectedClientId, "Nickname is already used");
        }
        _world.PlayerNickByPeerId.Add(connectedClientId, nick); 
        
        EndSyncOnClient(connectedClientId, _world.Data.Serialize());
    }

    private void EndSyncOnClient(int id, byte[] serializableData) => RpcId(id, MethodName.EndSyncOnClientRpc, serializableData);
    [Rpc(CallLocal = true)]
    private void EndSyncOnClientRpc(byte[] serializableData)
    {
        _world.Data.Deserialize(serializableData);
        SyncEndedOnClientEvent.Invoke();
    }
    
    private void RejectSyncOnClient(int id, string errorMessage) => RpcId(id, MethodName.RejectSyncOnClientRpc, errorMessage);
    [Rpc(CallLocal = true)] 
    private void RejectSyncOnClientRpc(string errorMessage)
    {
        SyncRejectOnClientEvent.Invoke(errorMessage);
    }
}