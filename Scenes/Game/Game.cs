using Fantoria.Lib.Nodes.Container;
using Fantoria.Scenes.Game.Net;
using Fantoria.Scenes.Game.Starters;
using Fantoria.Scenes.Screen.Hud;
using Godot;

namespace Fantoria.Scenes.Game;

public partial class Game : Node2D
{

    [Export] [NotNull] public NodeContainer WorldContainer { get; set; }
    [Export] [NotNull] public NodeContainer HudContainer { get; set; }
    [Export] [NotNull] public GamePackedScenes PackedScenes { get; set; }

    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }

    public void Init(BaseGameStarter gameStarter)
    {
        gameStarter.Init(this);
    }

    public World.World AddWorld()
    {
        World.World world = PackedScenes.World.Instantiate<World.World>();
        world.SetName("World");
        WorldContainer.ChangeStoredNode(world);
        return world;
    }
    
    public Hud AddHud()
    {
        Hud hud = PackedScenes.Hud.Instantiate<Hud>();
        hud.Init(WorldContainer.GetCurrentStoredNode<World.World>());
        HudContainer.ChangeStoredNode(hud);
        return hud;
    }

    public Network AddNetwork()
    {
        Network network = new Network();
        this.AddChildWithName(network, "Network");
        return network;
    }

    public Synchronizer AddSynchronizer()
    {
        Synchronizer synchronizer = new Synchronizer(this);
        this.AddChildWithName(synchronizer, "Synchronizer");
        return synchronizer;
    }
}