using System;
using Fantoria.Lib.Utils.Extensions;
using Godot;

namespace Fantoria.Lib;

public class LibInitializer
{

    private Func<Node, bool> _nodeNetworkExtensionsIsClientChecker;

    public LibInitializer SetNodeNetworkExtensionsIsClientChecker(Func<Node, bool> isClientChecker)
    {
        _nodeNetworkExtensionsIsClientChecker = isClientChecker;
        return this;
    }
    
    public void Init()
    {
        if (_nodeNetworkExtensionsIsClientChecker == null)
        {
            throw new Exception("You must set NodeNetworkExtensionsIsClientChecker before calling Init function");
        }
        
        ServiceLocator.Init();
        Log.Info("Lib initializing...");
        
        LibService.ExceptionHandler.AddExceptionHandlerForUnhandledException();
        LibService.CmdArgs.LogCmdArgs();
        NodeNetworkExtensionsState.SetIsClientChecker(_nodeNetworkExtensionsIsClientChecker);
    }
}