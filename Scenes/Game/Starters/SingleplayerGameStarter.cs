using Fantoria.Scenes.Screen.Hud;
using Fantoria.Scripts.Content.LoadingScreen;

namespace Fantoria.Scenes.Game.Starters;

public class SingleplayerGameStarter : BaseGameStarter
{
    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        
        game.AddWorld();
        game.AddHud();
        
        Service.LoadingScreen.Clear();
    }
}