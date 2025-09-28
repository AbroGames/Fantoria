using Fantoria.Scripts.Content.LoadingScreen;
using Fantoria.Scripts.Services.Settings;

namespace Fantoria.Scenes.Game.Starters;

public class SingleplayerGameStarter : BaseGameStarter
{
    
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        PlayerSettings playerSettings = Service.PlayerSettings.GetPlayerSettings();
        World.World world = game.AddWorld();
        Synchronizer synchronizer = game.AddSynchronizer(playerSettings);
        game.AddHud();
        
        world.StartStopService.StartNewGame(playerSettings.Nick);
        synchronizer.StartSyncOnClient();
    }
}