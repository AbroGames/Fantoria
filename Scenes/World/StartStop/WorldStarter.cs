namespace Fantoria.Scenes.World.StartStop;

public class WorldStarter(World world)
{

    public void StartOnServer(string saveFilePath = null)
    {
        if (saveFilePath != null)
        {
            world.SaveFilePath = saveFilePath;
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
        world.AddBattleSurface();
        world.AddBattleSurface();
        world.AddPlayersData();
    }
}