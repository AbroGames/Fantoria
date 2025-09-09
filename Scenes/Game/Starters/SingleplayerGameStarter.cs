using Fantoria.Scripts.Content.LoadingScreen;

namespace Fantoria.Scenes.Game.Starters;

public class SingleplayerGameStarter : BaseGameStarter
{
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);

        Synchronizer synchronizer = game.AddSynchronizer();
        World.World world = game.AddWorld();
        game.AddHud();
        
        world.InitOnServer();
        synchronizer.StartSyncOnClient();
        Service.LoadingScreen.Clear();
    }
}