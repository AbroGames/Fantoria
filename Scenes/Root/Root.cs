using Fantoria.Lib.Nodes.Container;
using Fantoria.Scenes.Root.Starters;
using Godot;

namespace Fantoria.Scenes.Root;

public partial class Root : Node2D
{
    
    [Export] [NotNull] public NodeContainer MainSceneContainer { get; set; }
    [Export] [NotNull] public NodeContainer LoadingScreenContainer { get; set; }
    [Export] [NotNull] public PackedScenes PackedScenes { get; set; }

    private RootStarterManager _rootStarterManager;
    
    public override void _Ready()
    {
        Callable.From(() => {
            Init();
            Start();
        }).CallDeferred();
    }

    private void Init()
    {
        _rootStarterManager = new RootStarterManager(this);
        _rootStarterManager.Init();
        NotNullChecker.CheckProperties(this); // We call NotNullChecker here, because it has not been created earlier
    }

    private void Start()
    {
        _rootStarterManager.Start();
    }
}
