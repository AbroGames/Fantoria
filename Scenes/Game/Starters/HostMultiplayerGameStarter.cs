using Fantoria.Scripts.Content.LoadingScreen;

namespace Fantoria.Scenes.Game.Starters;

public class HostMultiplayerGameStarter(int? port = null, string adminNickname = null, int? parentPid = null) : BaseGameStarter
{

    public override void Init(Game game)
    {
        base.Init(game);
        Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        
        //TODO Добавить ProcessShutdowner, если передан parentPid 
        //TODO тут два варианта ветвления: отдельный стартер для DedicatedServer, или if (isClient()). По идее далее будет использоваться только второй вариант.
    }
}