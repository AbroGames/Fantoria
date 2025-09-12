using System.Collections.Generic;
using Godot;

namespace Fantoria.Scenes.World.Data;

public partial class WorldPersistenceData : Node
{
    
    public string SaveFilePath { get; set; } // World will be to save to this file on server
    
    public Dictionary<string, PlayerData> PlayerByNick = new();
   /* private string _saveFilePath;
    public string SaveFilePath
    {
        get => _saveFilePath;
        set
        {
            _saveFilePath = value;
            Rpc(nameof(RpcSetSaveFilePath), value); // отправляем всем игрокам
        }
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void RpcSetSaveFilePath(string path)
    {
        SaveFilePath = path;
    }*/
    
}