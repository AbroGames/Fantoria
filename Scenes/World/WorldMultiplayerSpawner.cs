using Godot;

namespace Fantoria.Scenes.World;

//TODO This node and scene in Lib utils nodes (as SelfMultiplayerSpawner) + AbstractPackedScenes (with Scenes field) to Lib utils nodes, change here WorldPackedScenes to AbstractPackedScenes
//TODO Для SelfMultiplayerSpawner: Необходимо создать наслденика сцены, наследника класса, добавить туда PackedScenes со всеми требуемыми нодами. Важно, чтобы в них не было самой SelfMultiplayerSpawner.
public partial class WorldMultiplayerSpawner : MultiplayerSpawner
{
    [Export] [NotNull] public WorldPackedScenes PackedScenes { get; set; }
    
    private Node _initSpawnPath;
    private bool _selfSync;
    
    public WorldMultiplayerSpawner Init(Node spawnPath, bool selfSync = true)
    {
        _initSpawnPath = spawnPath;
        _selfSync = selfSync;
        return this;
    }

    public override void _Ready()
    {
        foreach (var packedScene in PackedScenes.Scenes)
        {
            AddSpawnableScene(packedScene.ResourcePath);
        }
        if (_selfSync) 
        {
            AddSpawnableScene(SceneFilePath); // Reference by self
        }
        
        if (_initSpawnPath != null) // _initSpawnPath can be null, if Spawner was synced on network by another Spawner
        {
            SetSpawnPath(GetPathTo(_initSpawnPath));
        }
    }
}