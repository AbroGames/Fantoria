using Godot;

namespace Fantoria.Scenes.World.Entity.Building;

public partial class MapPoint : Node2D
{

    [Export] public int a { get; set; } //TODO del
    [Export] protected int b;
    [Export] public int c;
    
    public override void _Ready()
    {
        Log.Warning("Position: " + Position + " ; " + c);
        if (this.IsServer()) c = 10;
    }
}