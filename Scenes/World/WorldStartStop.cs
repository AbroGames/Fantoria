namespace Fantoria.Scenes.World;

public class WorldStartStop(World world)
{
    
    public void StartNewGame(string adminNickname = null)
    {
        ServerInit(adminNickname);
        world.Tree.AddMapSurface();
    }
    
    public void LoadGame(string saveFileName, string adminNickname = null)
    {
        ServerInit(adminNickname);
        world.Data.SaveLoad.Load(saveFileName);
        
        //TODO Restore World state from data
    }

    private void ServerInit(string adminNickname = null)
    {
        world.MainAdminNick = adminNickname;
        world.StateChecker.InitOnServer();
    }
    
    public void Shutdown()
    {
        world.Data.SaveLoad.AutoSave();
    }
}