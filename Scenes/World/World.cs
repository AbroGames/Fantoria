using Godot;

namespace Fantoria.Scenes.World;

public partial class World : Node2D
{

    //TODO PlayerInfo, Map, List<Battles>
    public SaveLoad SaveLoad { get; } = new();
    
    public override void _Notification(int id)
    {
        if (id == NotificationExitTree) Shutdown();
    }

    private void Shutdown()
    {
        //TODO Сохранение состояния игры в файл и т.д.  
    }
}