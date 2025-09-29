namespace Fantoria.Scenes.World.Data;

public interface ISerializableStorage
{
    
    public byte[] SerializeStorage();
    public void DeserializeStorage(byte[] storageBytes);
    public void SetAllPropertyListeners();
}