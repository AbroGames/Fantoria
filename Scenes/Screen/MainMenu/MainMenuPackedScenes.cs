using Godot;

namespace Fantoria.Scenes.Screen.MainMenu;

public partial class MainMenuPackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene Main { get; private set; }
    [Export] [NotNull] public PackedScene CreateServer { get; private set; }
    [Export] [NotNull] public PackedScene ConnectToServer { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }
}