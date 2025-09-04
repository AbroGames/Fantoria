using Godot;

namespace Fantoria.Scenes.Screen.Hud;

public partial class Hud : Control
{
    
    private World.World _world;
    
    public void Init(World.World world)
    {
        if (world == null) Log.Error("World must be not null");
        _world = world;
    }
}