using System.Security.Cryptography;
using System.Text;
using Fantoria.Scenes.World.Data.General;
using Fantoria.Scenes.World.Data.Player;
using Godot;

namespace Fantoria.Scenes.World.Data;

public partial class WorldPersistenceData : Node
{
    
    [Export] [NotNull] public GeneralDataStorage General { get; private set; }
    [Export] [NotNull] public PlayerDataStorage Players { get; private set; }
    
    public WorldDataSaveLoad SaveLoad;
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);

        SaveLoad = new(this);
    }
    
    public byte[] Serialize()
    {
        return new byte[1]; //TODO MessagePack.Serialize
    }
    
    public void Deserialize(byte[] serializableData = null)
    { 
        //TODO Data = MessagePack.Deserialize
    }

    public string GetFullData()
    {
        return "FULL INFO"; //TODO
    }

    public string GetDataHash()
    {
        byte[] hashBytes = MD5.HashData(Serialize());

        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}