using Fantoria.Lib.Nodes.Process;
using Fantoria.Scripts.Content.LoadingScreen;

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
        
        game.Network.HostServer(port ?? DefaultPort);
        
        //TODO дальше два варианта ветвления: отдельный стартер для DedicatedServer, или if (isClient()). По идее далее будет использоваться только второй вариант.
    }
}