using Fantoria.Lib;

namespace Fantoria.Scenes.Root.Starters;

public abstract class BaseRootStarter
{

    public virtual void Init(Root root)
    {
        // We can't log anything before Lib initialized
        new LibInitializer()
            .SetNodeNetworkExtensionsIsClientChecker(_ => !Service.CmdArgs.IsDedicatedServer) // IsDedicatedServer is null now, but will be set before the lambda is called
            .Init();
        
        Log.Info("Initializing base...");
        
        Service.LoadingScreen.Init(root.LoadingScreenContainer, null/*TODO*/);
        Service.MainScene.Init(root.MainSceneContainer, null/*TODO*/);
    }

    public virtual void Start(Root root)
    {
        Log.Info("Starting base...");
    }
}