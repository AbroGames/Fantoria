using Fantoria.Scenes.World.Data.General;
using Fantoria.Scenes.World.Data.MapPoint;
using Fantoria.Scenes.World.Data.Player;
using Godot;

namespace Fantoria.Scenes.World.Data;

public partial class WorldPersistenceData : Node
{
    
    [Export] [NotNull] public GeneralDataStorage General { get; private set; }
    [Export] [NotNull] public PlayerDataStorage Players { get; private set; }
    [Export] [NotNull] public MapPointDataStorage MapPoint { get; private set; }
    
    public WorldDataSaveLoad SaveLoad;
    public WorldDataSerializer Serializer;
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);

        SaveLoad = new(this);
        Serializer = new(this);
    }
}