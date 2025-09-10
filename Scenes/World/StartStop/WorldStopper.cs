namespace Fantoria.Scenes.World.StartStop;

public class WorldStopper(World world)
{
    
    public void StopOnServer()
    {
        new WorldSaveLoad(world).SaveToDiskOnServer();
        //TODO gracefully shutdown
    }
}