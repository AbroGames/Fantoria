namespace Fantoria.Scenes.World;

public class WorldStartStop(World world)
{
    
    public void StartNewGame()
    {
        ServerInit();
        world.AddMapSurface();
    }
    
    public void LoadGame(string saveFileName)
    {
        ServerInit();
        world.Data.SaveLoad.Load(saveFileName);
        
        //TODO Restore World state on data
    }

    private void ServerInit()
    {
        world.StateChecker.InitOnServer();
    }
    
    public void Shutdown()
    {
        world.Data.SaveLoad.AutoSave();
    }
}