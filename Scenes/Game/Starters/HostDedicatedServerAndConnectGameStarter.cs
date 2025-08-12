using Fantoria.Lib.Nodes.Process;

namespace Fantoria.Scenes.Game.Starters;

public class HostDedicatedServerAndConnectGameStarter(int? port = null, string adminNickname = null, bool? showGui = null) : ConnectToMultiplayerGameStarter(Localhost, port)
{
    private readonly int? _port = port;

    public override void Init(Game game)
    {
        int dedicatedServerPid = Service.Process.StartNewDedicatedServerApplication(
            _port.GetValueOrDefault(DefaultPort), 
            adminNickname, 
            showGui.GetValueOrDefault(true));
        
        ProcessShutdowner dedicatedServerShutdowner = new ProcessShutdowner() 
            .Init(dedicatedServerPid, pid => $"Kill server process: {pid}."); 
        game.AddChild(dedicatedServerShutdowner); 
        
        base.Init(game); // Try to connect to new hosted server
    }
}