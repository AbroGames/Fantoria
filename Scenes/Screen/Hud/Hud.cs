﻿using Fantoria.Scenes.Game;
using Fantoria.Scripts.Content.LoadingScreen;
using Godot;

namespace Fantoria.Scenes.Screen.Hud;

public partial class Hud : Control
{
    
    [Export] [NotNull] public Button Test1 { get; set; }
    [Export] [NotNull] public Button Test2 { get; set; }
    [Export] [NotNull] public Button Test3 { get; set; }
    [Export] [NotNull] public Button LogChildren { get; set; }
    [Export] [NotNull] public Label Info { get; set; }
    
    private World.World _world;
    private Synchronizer _synchronizer;
    
    public Hud Init(World.World world, Synchronizer synchronizer)
    {
        if (world == null) Log.Error("World must be not null");
        _world = world;
        
        if (synchronizer == null) Log.Error("Synchronizer must be not null");
        _synchronizer = synchronizer;

        ConnectToEvents();
        
        return this;
    }

    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
        Test1.Pressed += () => { _world.Test1(); };
        Test2.Pressed += () => { _world.Test2(); };
        Test3.Pressed += () => { _world.Test3(); };
        LogChildren.Pressed += () => { _world.StateCheckerService.LogState(); _world.StateCheckerService.StateCheckRequest(); };
    }

    private void ConnectToEvents()
    {
        _synchronizer.SyncStartedOnClientEvent += () => Service.LoadingScreen.SetLoadingScreen(LoadingScreenTypes.Type.Loading);
        _synchronizer.SyncEndedOnClientEvent += () => Service.LoadingScreen.Clear();
        _synchronizer.SyncRejectOnClientEvent += (errorMessage) =>
        {
            Log.Error($"Synchronization with the server was rejected: {errorMessage}");
            Service.MainScene.StartMainMenu();
            //TODO Show error message in menu
            //TODO There are engine network errors in log, need fix
            Service.LoadingScreen.Clear();
        };
    }

    public override void _Process(double delta)
    {
        Info.Text = $"FPS: {Engine.GetFramesPerSecond():N0}";
        Info.Text += $"\nTPS: {Mathf.Min(1.0/Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess), Engine.PhysicsTicksPerSecond):N0}";
        
        Info.Text += $"\n\nNodes: {Performance.GetMonitor(Performance.Monitor.ObjectNodeCount)}";
        Info.Text += $"\nWorld 1-level nodes: {_world.GetChildCount()}";
        Info.Text += $"\nFrame time process: {Performance.GetMonitor(Performance.Monitor.TimeProcess)*1000:N1}ms";
        Info.Text += $"\nPhysics time process: {Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess)*1000:N1}ms ({Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * Engine.PhysicsTicksPerSecond * 100:N0} %)";
        Info.Text += $"\nNavigation time process: {Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess)*1000:N1}ms";
    }
}