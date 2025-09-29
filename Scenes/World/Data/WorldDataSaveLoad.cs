using Godot;

namespace Fantoria.Scenes.World.Data;

public class WorldDataSaveLoad(WorldPersistenceData worldData)
{

    private const string SaveDirPath = "user://saves/";
    private const string AutoSaveName = "auto.bin";
    
    public void Save(string saveFileName)
    {
        worldData.General.GeneralData.SaveFileName = saveFileName;
        SaveToDisk(worldData.Serializer.SerializeWorldData(), saveFileName);
    }
    
    public void AutoSave()
    {
        SaveToDisk(worldData.Serializer.SerializeWorldData(), AutoSaveName);
    }

    public void Load(string saveFileName)
    {
        byte[] data = LoadFromDisk(saveFileName);
        worldData.Serializer.DeserializeWorldData(data);
    }

    private void SaveToDisk(byte[] data, string saveFileName)
    {
        DirAccess.MakeDirRecursiveAbsolute(SaveDirPath);
        using var file = FileAccess.Open(SaveDirPath + saveFileName, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            Log.Error($"Failed to save file '{SaveDirPath + saveFileName}': {FileAccess.GetOpenError()}");
            return;
        }
        
        file.StoreBuffer(data);
        file.Close();
    }
    
    private byte[] LoadFromDisk(string saveFileName)
    {
        using var file = FileAccess.Open(SaveDirPath + saveFileName, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            Log.Error($"Failed to load file '{SaveDirPath + saveFileName}': {FileAccess.GetOpenError()}");
            return null;
        }
        
        byte[] data = file.GetBuffer((long) file.GetLength());
        file.Close();
        
        return data;
    }
}