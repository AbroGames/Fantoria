using System;
using System.Reflection;
using System.Text;
using Godot;
using Godot.Collections;

namespace Fantoria.Scenes.World.Data.PlayersData;

public partial class PlayersData : Node
{
    [Export] public Dictionary<int, string> NickByPeerId = new();
    
    public void LogData() => Rpc(MethodName.LogDataRpc);
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void LogDataRpc()
    {
        Log.Debug(this.GetExportMembersInfo());
    }
}