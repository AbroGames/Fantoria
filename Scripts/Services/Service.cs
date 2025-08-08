using Fantoria.Scripts.Services.CmdArgs;
using Fantoria.Scripts.Services.LoadingScreen;
using Fantoria.Scripts.Services.MainScene;
using Fantoria.Scripts.Services.Process;

namespace Fantoria.Scripts.Services;

public static class Service
{
    public static CmdArgsService CmdArgs => ServiceLocator.Get<CmdArgsService>();
    public static ProcessService Process => ServiceLocator.Get<ProcessService>();
    public static LoadingScreenService LoadingScreen => ServiceLocator.Get<LoadingScreenService>();
    public static MainSceneService MainScene => ServiceLocator.Get<MainSceneService>();

    public static class Global
    {
        
    }
}