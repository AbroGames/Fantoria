using Godot;

namespace Fantoria.Scenes.Game;

public partial class GamePackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene World { get; set; }
    [Export] [NotNull] public PackedScene Hud { get; set; }

    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this); 
    }
}