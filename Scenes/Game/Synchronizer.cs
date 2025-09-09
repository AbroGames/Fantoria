using Fantoria.Scripts.Content.LoadingScreen;
using Godot;

namespace Fantoria.Scenes.Game;

//TODO Мб перенести класс в World, т.к. данные игрока туда синкаются, а LoadingScreen можно дергать через назначаемые Action-ы
//TODO Но синхронайзер удобен тут тем, что он по разному вызывается из стартеров. Мб часть оставить тут, но отсюда просто вызывать Action-ы в World? В World.PlayerData?
//TODO Оставить тут логику скринов, или это в Hud? По идее Hud привязан к World только должен быть, и LoadingScreen -- это другой объект, а не Hud
public partial class Synchronizer : Node
{

    public void StartSyncOnClient()
    {
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        //TODO Take player info from Game/Settings
        NewClientInitOnServer();
    }

    private void NewClientInitOnServer() => RpcId(ServerId, MethodName.NewClientInitOnServerRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)] 
    private void NewClientInitOnServerRpc()
    {
        //TODO Receive and store nickname, color and etc
        //TODO Resend client info to another clients by MpSync in World
        SyncWorldDataOnClient(GetMultiplayer().GetRemoteSenderId());
    }

    private void SyncWorldDataOnClient(int id) => RpcId(id, MethodName.SyncWorldDataOnClientRpc);
    [Rpc(CallLocal = true)] 
    private void SyncWorldDataOnClientRpc()
    {
        Service.LoadingScreen.Clear();
    }
}