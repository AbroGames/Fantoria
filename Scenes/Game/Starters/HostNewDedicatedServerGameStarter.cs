using Fantoria.Lib.Nodes.Process;

namespace Fantoria.Scenes.Game.Starters;

public class HostNewDedicatedServerGameStarter(int? port = null, string adminNickname = null, bool? showGui = null) : ConnectToMultiplayerGameStarter(Localhost, port)
{
    private readonly int? _port = port;

    public override void Init(Game game)
    {
        //TODO замерить время создания нового процесса, и если долго то добавить сюда //Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        int dedicatedServerPid = Service.Process.StartNewDedicatedServerApplication(
            _port.GetValueOrDefault(DefaultPort), 
            adminNickname, 
            showGui.GetValueOrDefault(false));
        
        ProcessShutdowner dedicatedServerShutdowner = new ProcessShutdowner() //TODO это тоже затестить (в режиме showGui = true)
            .Init(dedicatedServerPid, pid => $"Kill server process: {pid}."); 
        game.AddChild(dedicatedServerShutdowner); 
        
        base.Init(game);
    }
}