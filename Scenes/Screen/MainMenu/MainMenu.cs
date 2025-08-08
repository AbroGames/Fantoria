using Fantoria.Lib.Nodes.Container;
using Godot;

namespace Fantoria.Scenes.Screen.MainMenu;

public partial class MainMenu : Node2D
{
    [Export] [NotNull] public NodeContainer BackgroundContainer { get; private set; }
    [Export] [NotNull] public NodeContainer MenuContainer { get; private set; }
	
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }
    
    //TODO в сервис? Но по сути тогда снова засинглтоним меню. В сервисы можно выносить только сцены/вещи существующие в любой момент времени игры (и в меню и в игре).
    //TODO Этот объект по сути эквивалент объекта Game
    public Node ChangeMenu(PackedScene newMenu)  
    {
        return MenuContainer.ChangeStoredNode(newMenu.Instantiate());
    }
}