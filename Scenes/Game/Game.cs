﻿using Fantoria.Lib.Nodes.Container;
using Fantoria.Scenes.Game.Net;
using Fantoria.Scenes.Game.Starters;
using Fantoria.Scenes.Screen.Hud;
using Fantoria.Scenes.World;
using Fantoria.Scripts.Services.Settings;
using Godot;

namespace Fantoria.Scenes.Game;

public partial class Game : Node2D
{

    [Export] [NotNull] public NodeContainer WorldContainer { get; set; }
    [Export] [NotNull] public NodeContainer HudContainer { get; set; }
    [Export] [NotNull] public GamePackedScenes PackedScenes { get; set; }

    private Network _network;
    private Synchronizer _synchronizer;

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
        Hud hud = PackedScenes.Hud.Instantiate<Hud>()
            .Init(WorldContainer.GetCurrentStoredNode<World.World>(), _synchronizer);
        hud.SetName("Hud");
        HudContainer.ChangeStoredNode(hud);
        return hud;
    }

    public Synchronizer AddSynchronizer(PlayerSettings playerSettings)
    {
        _synchronizer?.QueueFree();
        _synchronizer = new Synchronizer()
            .Init(WorldContainer.GetCurrentStoredNode<World.World>(), playerSettings);
        this.AddChildWithName(_synchronizer, "Synchronizer");
        return _synchronizer;
    }

    public Network AddNetwork()
    {
        _network?.QueueFree();
        _network = new Network();
        this.AddChildWithName(_network, "Network");
        return _network;
    }
}