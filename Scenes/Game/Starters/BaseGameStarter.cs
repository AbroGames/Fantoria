using Fantoria.Scripts.Content.LoadingScreen;

namespace Fantoria.Scenes.Game.Starters;

public abstract class BaseGameStarter
{
    public const string Localhost = "127.0.0.1";
    public const string DefaultHost = Localhost;
    public const int DefaultPort = 25566;

    public virtual void Init(Game game)
    {
        
    }
}
