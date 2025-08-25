using System;
using Fantoria.Lib.Nodes.Container;
using Fantoria.Lib.Nodes.Process;
using Fantoria.Scenes.Game;
using Fantoria.Scenes.Game.Starters;
using Godot;
using MainMenu = Fantoria.Scenes.Screen.MainMenu.MainMenu;

namespace Fantoria.Scripts.Services.MainScene;

[Service]
public class MainSceneService
{
    
    private NodeContainer _mainSceneContainer;
    private PackedScene _mainMenuPackedScene;

    public void Init(NodeContainer mainSceneContainer, PackedScene mainMenuPackedScene)
    {
        _mainSceneContainer = mainSceneContainer;
        _mainMenuPackedScene = mainMenuPackedScene;
    }
    
    public void StartMainMenu()
    {
        MainMenu mainMenu = _mainMenuPackedScene.Instantiate<MainMenu>();
        _mainSceneContainer.ChangeStoredNode(mainMenu);
    }
    
    public void StartSingleplayerGame()
    {
        Game game = new Game();
        _mainSceneContainer.ChangeStoredNode(game);
        
        game.Init(new SingleplayerGameStarter());
    }
    
    public void ConnectToMultiplayerGame(string host = null, int? port = null)
    {
        Game game = new Game();
        _mainSceneContainer.ChangeStoredNode(game);
        
        game.Init(new ConnectToMultiplayerGameStarter(host, port));
    }
    
    /// <summary>
    /// Start new server and connect to them. Use in client process.
    /// </summary>
    /// <param name="port">Port number on which the server will listen.</param>
    /// <param name="createDedicatedServerProcess">If true, create a new OS process running a dedicated server, and have this process connect to it as a client.</param>
    public void HostMultiplayerGameAsClient(int? port = null, bool? createDedicatedServerProcess = null)
    {
        Game game = new Game();
        _mainSceneContainer.ChangeStoredNode(game);
        
        string adminNickname = ""; //TODO current user nickname
        
        if (createDedicatedServerProcess ?? false)
        {
            game.Init(new HostDedicatedServerAndConnectGameStarter(port, adminNickname, true));
        }
        else
        {
            game.Init(new HostMultiplayerGameStarter(port, adminNickname));
        }
    }
    
    /// <summary>
    /// Start new server. Use in dedicated server process.
    /// </summary>
    /// <param name="port">Port number on which the server will listen.</param>
    /// <param name="adminNickname">This user can manage the server</param>
    /// <param name="parentPid">If this process is a dedicated server created from a client, use the PID of the client process.</param>
    public void HostMultiplayerGameAsDedicatedServer(int? port = null, string adminNickname = null, int? parentPid = null)
    {
        Game game = new Game();
        _mainSceneContainer.ChangeStoredNode(game);
        
        game.Init(new HostMultiplayerGameStarter(port, adminNickname, parentPid));
    }
    
    [Obsolete("Global accessor to MainMenu, like Singleton")] //TODO удалить методы, если не потребуются
    public MainMenu GetMainMenu()
    {
        return _mainSceneContainer.GetCurrentStoredNode<MainMenu>();
    }

    [Obsolete("Global accessor to Game, like Singleton")]
    public Game GetGame()
    {
        return _mainSceneContainer.GetCurrentStoredNode<Game>();
    }

    public bool MainSceneIsMainMenu()
    {
        return _mainSceneContainer.GetCurrentStoredNode<Node>() is MainMenu;
    }

    public bool MainSceneIsGame()
    {
        return _mainSceneContainer.GetCurrentStoredNode<Node>() is Game;
    }
    
    public void Shutdown()
    {
        _mainSceneContainer.GetTree().Root.PropagateNotification((int) Node.NotificationExitTree); // Notify all nodes about game closing
        _mainSceneContainer.GetTree().Quit();
    }
}