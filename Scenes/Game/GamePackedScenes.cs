using Godot;

namespace Fantoria.Scenes.Game;

public partial class GamePackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene World { get; private set; }
    [Export] [NotNull] public PackedScene Hud { get; private set; }

    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this); 
    }
}