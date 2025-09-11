using System;
using Godot;

namespace Fantoria.Scenes.Game;

//TODO Мб перенести класс в World, т.к. данные игрока туда синкаются, а LoadingScreen можно дергать через назначаемые Action-ы
//TODO Но синхронайзер удобен тут тем, что он по разному вызывается из стартеров. Мб часть оставить тут, но отсюда просто вызывать Action-ы в World? В World.PlayerData?
//TODO Оставить тут логику скринов, или это в Hud? По идее Hud привязан к World только должен быть, и LoadingScreen -- это другой объект, а не Hud

/// <summary>
/// Use for send player data (like nick, color etc.) to server.
/// We must use synchronizer out of World, because in connecting process World children nodes don't exist.
/// </summary>
public partial class Synchronizer : Node
{

    public Action SyncStartedOnClientEvent;
    public Action SyncEndedOnClientEvent;
    
    private World.World _world;
    
    public Synchronizer(World.World world)
    {
        if (world == null) Log.Error("World must be not null");
        _world = world;
    }
    
    public void StartSyncOnClient()
    {
        SyncStartedOnClientEvent.Invoke();
        //TODO Take player info from Game/Settings
        NewClientInitOnServer();
    }

    private void NewClientInitOnServer() => RpcId(ServerId, MethodName.NewClientInitOnServerRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)] 
    private void NewClientInitOnServerRpc()
    {
        //TODO Receive and store nickname, color and etc in World.PlayerData. Client info automatically resends to another clients by MpSync.
        int connectedClientId = GetMultiplayer().GetRemoteSenderId();
        _world.PlayersData.NickByPeerId.Add(connectedClientId, "TestNick"); //TODO Change to real nick
        EndSyncOnClient(connectedClientId);
    }

    private void EndSyncOnClient(int id) => RpcId(id, MethodName.EndSyncOnClientRpc);
    [Rpc(CallLocal = true)] 
    private void EndSyncOnClientRpc()
    {
        SyncEndedOnClientEvent.Invoke();
    }
}