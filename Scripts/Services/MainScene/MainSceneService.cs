﻿using Fantoria.Lib.Nodes.Container;
using Fantoria.Scenes.Game;
using Fantoria.Scenes.Game.Starters;
using Godot;
using MainMenu = Fantoria.Scenes.Screen.MainMenu.MainMenu;

namespace Fantoria.Scripts.Services.MainScene;

[Service]
public class MainSceneService
{
    
    private NodeContainer _mainSceneContainer;
    private PackedScene _gamePackedScene;
    private PackedScene _mainMenuPackedScene;

    public void Init(NodeContainer mainSceneContainer, PackedScene gamePackedScene, PackedScene mainMenuPackedScene)
    {
        _mainSceneContainer = mainSceneContainer;
        _gamePackedScene = gamePackedScene;
        _mainMenuPackedScene = mainMenuPackedScene;
    }
    
    public void StartMainMenu()
    {
        MainMenu mainMenu = _mainMenuPackedScene.Instantiate<MainMenu>();
        _mainSceneContainer.ChangeStoredNode(mainMenu);
    }
    
    public void StartSingleplayerGame()
    {
        Game game = _gamePackedScene.Instantiate<Game>();
        game.SetName("Game");
        _mainSceneContainer.ChangeStoredNode(game);
        
        game.Init(new SingleplayerGameStarter());
    }
    
    public void ConnectToMultiplayerGame(string host = null, int? port = null)
    {
        Game game = _gamePackedScene.Instantiate<Game>();
        game.SetName("Game");
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
        Game game = _gamePackedScene.Instantiate<Game>();
        game.SetName("Game");
        _mainSceneContainer.ChangeStoredNode(game);
        
        string adminNickname = ""; //TODO current user nickname. Сделать по аналогии с ником/цветом для синхронайзера
        
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
    /// <param name="gameRender">Show not the GUI, but the game scene</param>
    public void HostMultiplayerGameAsDedicatedServer(int? port = null, string adminNickname = null, int? parentPid = null, bool? gameRender = null)
    {
        Game game = _gamePackedScene.Instantiate<Game>();
        game.SetName("Game");
        _mainSceneContainer.ChangeStoredNode(game);
        
        game.Init(new HostMultiplayerGameStarter(port, adminNickname, parentPid));
        
        Service.LoadingScreen.Clear();
        if (!gameRender.HasValue || !gameRender.Value)
        {
            //TODO Show GUI with stats
        }
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