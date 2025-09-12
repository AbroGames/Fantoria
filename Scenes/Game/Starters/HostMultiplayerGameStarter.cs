using Fantoria.Lib.Nodes.Process;
using Fantoria.Scenes.Game.Net;
using Fantoria.Scenes.World;
using Fantoria.Scenes.World.StartStop;
using Fantoria.Scripts.Content.LoadingScreen;
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
        
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer();
        game.DoClient(() => game.AddHud());
        Network network = game.AddNetwork();
        
        Error error = network.HostServer(port ?? DefaultPort);
        if (error != Error.Ok)
        {
            game.DoClient(HostFailedEventOnClient);
            return;
        }

        new WorldStarter(world).StartOnServer(); //TODO Сделать так, чтобы клиент не мог подключиться до инита/загрузки мира: создавать до хоста, хостить с недоступным Bind IP, а потом менять на *. Резать подключение в синхронайзере.
        game.DoClient(synchronizer.StartSyncOnClient);
        Service.LoadingScreen.Clear(); //TODO Remove it or use only for dedicated server test (in server+client mode we clear screen in synchronizer) 
    }
    
    private void HostFailedEventOnClient()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show error in menu (it is client). Log already has error.
        Service.LoadingScreen.Clear();
    }
}