using Godot;

using static MessagePack.MessagePackSerializer;

namespace Fantoria.Scenes.World.Data.General;

public partial class GeneralDataStorage : Node
{

    public GeneralData GeneralData { get; private set; }

    public GeneralDataStorage()
    {
        AddGeneralData(new GeneralData());
    }

    private void AddGeneralData(GeneralData general)
    {
        GeneralData = general;
        GeneralData.PropertyChanged += (g, _) => UpdateGeneral((GeneralData) g);
    }

    private void UpdateGeneral(GeneralData general) => Rpc(MethodName.UpdateGeneralRpc, Serialize(general));
    [Rpc(CallLocal = false)]
    private void UpdateGeneralRpc(byte[] generalBytes)
    {
        GeneralData general = Deserialize<GeneralData>(generalBytes);
        AddGeneralData(general);
    }
    
}