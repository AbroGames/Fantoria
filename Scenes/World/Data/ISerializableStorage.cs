namespace Fantoria.Scenes.World.Data;

using static MessagePack.MessagePackSerializer;

public interface ISerializableStorage
{
    
    public byte[] SerializeStorage();
    public void DeserializeStorage(byte[] storageBytes);
    public void SetAllPropertyListeners();
}