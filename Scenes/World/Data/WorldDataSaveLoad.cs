using Godot;

namespace Fantoria.Scenes.World.Data;

public class WorldDataSaveLoad(WorldPersistenceData worldData)
{

    private const string SaveDirPath = "user://saves/";
    private const string AutoSavePath = SaveDirPath + "auto.bin";
    
    public void Save(string saveFileName)
    {
        worldData.General.GeneralData.SaveFileName = saveFileName;
        SaveToDisk(worldData.Serialize(), saveFileName);
    }
    
    public void AutoSave()
    {
        SaveToDisk(worldData.Serialize(), AutoSavePath);
    }

    public void Load(string saveFileName)
    {
        byte[] data = LoadFromDisk(saveFileName);
        worldData.Deserialize(data);
    }

    private void SaveToDisk(byte[] data, string saveFileName)
    {
        DirAccess.MakeDirRecursiveAbsolute(SaveDirPath);
        using var file = FileAccess.Open(SaveDirPath + saveFileName, FileAccess.ModeFlags.Write);
        file.StoreBuffer(data);
        file.Close();
    }
    
    private byte[] LoadFromDisk(string saveFileName)
    {
        using var file = FileAccess.Open(SaveDirPath + saveFileName, FileAccess.ModeFlags.Read);
        byte[] data = file.GetBuffer((long) file.GetLength());
        file.Close();
        
        return data;
    }
}