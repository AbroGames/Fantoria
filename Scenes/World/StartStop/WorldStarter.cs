namespace Fantoria.Scenes.World.StartStop;

public class WorldStarter(World world)
{

    public void StartOnServer(string saveFilePath = null)
    {
        world.AddPersistenceData();
        if (saveFilePath != null)
        {
            world.Data.SaveFilePath = saveFilePath;
            new WorldSaveLoad(world).LoadFromDiskOnServer();
        }
        else
        {
            StartNewGameOnServer();
        }
    }

    private void StartNewGameOnServer()
    {
        world.AddMapSurface();
    }
}