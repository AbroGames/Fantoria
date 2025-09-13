using Fantoria.Scenes.Game.Net;
using Fantoria.Scripts.Content.LoadingScreen;
using Fantoria.Scripts.Services.Settings;
using Godot;

namespace Fantoria.Scenes.Game.Starters;

public class ConnectToMultiplayerGameStarter(string host = null, int? port = null) : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Connecting);
        
        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.AddHud();
        Network network = game.AddNetwork();
        
        game.GetMultiplayer().ConnectedToServer += synchronizer.StartSyncOnClient;
        game.GetMultiplayer().ConnectionFailed += ConnectionFailedEvent;
        
        Error error = network.ConnectToServer(host ?? DefaultHost, port ?? DefaultPort);
        if (error != Error.Ok)
        {
            ConnectionFailedEvent();
            return;
        }
    }

    private void ConnectionFailedEvent()
    {
        Service.MainScene.StartMainMenu();
        //TODO Show error in menu (it is client). Log already has error.
        Service.LoadingScreen.Clear();
    }
}