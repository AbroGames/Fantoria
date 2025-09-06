using Fantoria.Lib.Nodes.Process;
using Fantoria.Scenes.Game.Net;
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
        
        Network network = game.AddNetwork();
        game.AddSynchronizer();
        game.AddWorld();
        game.DoClient(() => game.AddHud());
        
        Error error = network.HostServer(port ?? DefaultPort);
        if (error != Error.Ok)
        {
            HostFailedEvent();
            return;
        }
        Service.LoadingScreen.Clear();
    }
    
    private void HostFailedEvent()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show error in menu, if IsClient(). Log always have error.
        Service.LoadingScreen.Clear();
    }
}