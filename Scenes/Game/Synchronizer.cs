using Fantoria.Scripts.Content.LoadingScreen;
using Godot;

namespace Fantoria.Scenes.Game;

public partial class Synchronizer : Node
{
    
    private Game _game;

    public override void _Ready()
    {
        _game = GetParent<Game>();
    }

    public void ConnectedToServerEvent()
    {
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        //TODO Take player info from Game/Settings
        NewClientInitOnServer();
    }

    private void NewClientInitOnServer() => Rpc(MethodName.NewClientInitOnServerRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void NewClientInitOnServerRpc()
    {
        //TODO Receive and store nickname, color and etc
        //TODO Resend client info to another clients. Use this method?
        SyncWorldDataOnClient(GetMultiplayer().GetRemoteSenderId());
    }

    private void SyncWorldDataOnClient(int id) => RpcId(id, MethodName.SyncWorldDataOnClientRpc);
    [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void SyncWorldDataOnClientRpc()
    {
        _game.AddWorld();
        _game.AddHud();
        //TODO Receive world state as gzip json byte array from server SaveLoad system, and load it into client SaveLoad system
        Service.LoadingScreen.Clear();
    }
}