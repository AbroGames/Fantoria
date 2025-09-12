using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fantoria.Scenes.World.Data.General;
using Fantoria.Scenes.World.Data.Player;
using Godot;

namespace Fantoria.Scenes.World.Data;

public partial class WorldPersistenceData : Node
{
    
    [Export] [NotNull] public GeneralDataStorage General { get; private set; }
    [Export] [NotNull] public PlayerDataStorage Players { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }
    
    public byte[] Serialize()
    {
        return new byte[1]; //TODO MessagePack.Serialize
    }
    
    public void Deserialize(byte[] serializableData = null)
    { 
        //TODO Data = MessagePack.Deserialize
    }
}