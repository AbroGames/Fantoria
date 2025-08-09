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
        Game game = new Game().Init(new SingleplayerGameStarter());
        _mainSceneContainer.ChangeStoredNode(game);
    }
    
    public void ConnectToMultiplayerGame(string host = null, int? port = null)
    {
        Game game = new Game().Init(new ConnectToMultiplayerGameStarter(host, port));
        _mainSceneContainer.ChangeStoredNode(game);
    }
    
    public void HostMultiplayerGame(int? port = null, string adminNickname = null)
    {
        Game game = new Game().Init(new HostMultiplayerGameStarter(port, adminNickname));
        _mainSceneContainer.ChangeStoredNode(game);
    }
    
    public void HostNewDedicatedServerGame(int? port = null, string adminNickname = null, bool? showGui = null)
    {
        Game game = new Game().Init(new HostNewDedicatedServerGameStarter(port, adminNickname, showGui));
        _mainSceneContainer.ChangeStoredNode(game);
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
}