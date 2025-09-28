using Fantoria.Lib.Nodes.Process;
using Fantoria.Scenes.Game.Net;
using Fantoria.Scripts.Content.LoadingScreen;
using Fantoria.Scripts.Services.Settings;
using Godot;

namespace Fantoria.Scenes.Game.Starters;

public class HostMultiplayerGameStarter(int? port = null, string adminNickname = null, int? parentPid = null) : BaseGameStarter
{

    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        if (parentPid.HasValue)
        {
            ProcessDeadChecker clientDeadChecker = new ProcessDeadChecker(
                parentPid.Value, 
                () => Service.MainScene.Shutdown(),
                pid => $"Parent process {pid} is dead. Shutdown server.");
            game.AddChild(clientDeadChecker);
        }
        
        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.DoClient(() => game.AddHud());
        Network network = game.AddNetwork();
        
        Error error = network.HostServer(port ?? DefaultPort);
        if (error != Error.Ok)
        {
            game.DoClient(HostFailedEventOnClient);
            return;
        }

        //TODO Надо чтобы Network.Host был до запуска мира, т.к. требуется IsServer = true (хотя в Offline он тоже может быть true)
        //TODO При Shutdown Network и World я хочу, чтобы сервер сохранил мир, а клиент нет. Могу ли я настроить порядок Shutdown-ов?
        //TODO Или как-то поменять логику дефолтного Peer, чтобы было Server = false. Или проверку Node.IsServer делать с peer != null. Но в этом случае разное поведение двух IsServer.
        //TODO И вообще вынести это из NodeExtension в сервис global Network (вне либы)? Net.IsServer() ? 
        world.StartStopService.StartNewGame(adminNickname); //TODO Сделать так, чтобы клиент не мог подключиться до инита/загрузки мира: создавать до хоста, хостить с недоступным Bind IP, а потом менять на *. Резать подключение в синхронайзере.
        game.DoClient(synchronizer.StartSyncOnClient);
    }
    
    private void HostFailedEventOnClient()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show error in menu (it is client). Log already has error.
        Service.LoadingScreen.Clear();
    }
}