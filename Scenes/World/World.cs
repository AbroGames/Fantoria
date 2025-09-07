using Fantoria.Scenes.World.Entity.Building;
using Fantoria.Scenes.World.Surface.Map;
using Godot;

namespace Fantoria.Scenes.World;

public partial class World : Node2D
{
    
    //TODO WorldPackedScenes переименовать в SyncPackedScenes? И отдельно сделать ClientPackedScenes, который не привязан в MpSpawner. В нем всякие эффекты (не гуи, а мировые).
    //TODO А может лучше всё-таки оставить World ПОЛНОСТЬЮ синхронным, а World-эффекты вешать в отдельный слой, по соседству с World. Принцип как у HUD.
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; set; }
    [Export] [NotNull] public PackedScene WorldMultiplayerSpawnerPackedScene { get; private set; }

    //TODO PlayerInfo, Map, List<Battles>
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }

    public void InitOnServer()
    {
        MapSurface surface = PackedScenes.MapSurface.Instantiate<MapSurface>();
        this.AddChildWithName(surface, "Surface");
        
        WorldMultiplayerSpawner worldMultiplayerSpawner = WorldMultiplayerSpawnerPackedScene.Instantiate<WorldMultiplayerSpawner>()
            .Init(surface);
        this.AddChildWithName(worldMultiplayerSpawner, "MultiplayerSpawner");
    }
    
    //TODO Метод для создания Surface + Spawner

    public void CreatePoint() => RpcId(ServerId, MethodName.CreatePointRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void CreatePointRpc()
    {
        Log.Warning("Create point RPC");

        for (int i = 0; i < 10; i++)
        {
            MapSurface surface = PackedScenes.MapSurface.Instantiate<MapSurface>();
            this.AddChildWithUniqueName(surface, "Surface");
        
            WorldMultiplayerSpawner worldMultiplayerSpawner = WorldMultiplayerSpawnerPackedScene.Instantiate<WorldMultiplayerSpawner>()
                .Init(surface);
            this.AddChildWithUniqueName(worldMultiplayerSpawner, "MultiplayerSpawner");
            
            for (int j = 0; j < 10; j++)
            {
                MapPoint point = PackedScenes.MapPoint.Instantiate<MapPoint>();
                point.Position = Vec(LibService.Rand.Range(0, 1000), LibService.Rand.Range(0, 500));
                surface.AddChildWithUniqueName(point, "Point");
            }
        }
    }

    public void LogChildren() => Rpc(MethodName.LogChildrenRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void LogChildrenRpc()
    {
        Log.Info(this.GetFullTree());
        Log.Info(this.GetTreeHash());
    }

    public override void _Notification(int id)
    {
        if (id == NotificationExitTree) Shutdown();
    }

    private void Shutdown()
    {
        //TODO Сохранение состояния игры в файл и т.д.  
    }
}