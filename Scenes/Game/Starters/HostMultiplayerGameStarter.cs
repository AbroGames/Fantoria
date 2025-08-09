namespace Fantoria.Scenes.Game.Starters;

public class HostMultiplayerGameStarter(int? port = null, string adminNickname = null) : BaseGameStarter
{

    public override void Init(Game game)
    {
        //TODO тут два варианта ветвления: отдельный стартер для DedicatedServer, или if (isClient()). По идее далее будет использоваться только второй вариант.
    }
}