using Godot;

namespace Fantoria.Scenes.Root;

public partial class RootPackedScenes : Node
{
    
    [Export] [NotNull] public PackedScene Game { get; private set; }
    [Export] [NotNull] public PackedScene MainMenu { get; private set; }
    [Export] [NotNull] public PackedScene LoadingScreen { get; private set; }

    public void Init()
    {
        NotNullChecker.CheckProperties(this); // We call NotNullChecker here, because it has not been created in _Ready
    }
}