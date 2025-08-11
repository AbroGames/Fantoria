using Godot;

namespace Fantoria.Scenes.Root;

public partial class PackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene MainMenu { get; set; }
    [Export] [NotNull] public PackedScene LoadingScreen { get; set; }

    public void Init()
    {
        NotNullChecker.CheckProperties(this); // We call NotNullChecker here, because it has not been created in _Ready
    }
}