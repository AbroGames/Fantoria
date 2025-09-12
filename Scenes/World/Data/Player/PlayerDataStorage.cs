using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

using static MessagePack.MessagePackSerializer;

namespace Fantoria.Scenes.World.Data.Player;

public partial class PlayerDataStorage : Node
{
    
    public IReadOnlyDictionary<string, PlayerData> PlayerByNick => new ReadOnlyDictionary<string, PlayerData>(_playerByNick);
    private readonly Dictionary<string, PlayerData> _playerByNick = new();

    public void AddPlayer(PlayerData player)
    {
        AddPlayerLocal(player);
        Rpc(MethodName.AddPlayerRpc, Serialize(player));
    }
    
    [Rpc(CallLocal = false)]
    private void AddPlayerRpc(byte[] playerBytes) => AddPlayerLocal(Deserialize<PlayerData>(playerBytes));

    private void AddPlayerLocal(PlayerData player)
    {
        player.PropertyChanged += (p, _) => UpdatePlayer((PlayerData) p);
        _playerByNick[player.Nick] = player;
    }
    
    public void RemovePlayer(PlayerData player) => RemovePlayer(player.Nick);
    public void RemovePlayer(string nick) => Rpc(MethodName.RemovePlayerRpc, nick);
    [Rpc(CallLocal = true)]
    private void RemovePlayerRpc(string nick)
    {
        _playerByNick.Remove(nick);
    }
    
    private void UpdatePlayer(PlayerData player) => Rpc(MethodName.UpdatePlayerRpc, Serialize(player));
    [Rpc(CallLocal = false)]
    private void UpdatePlayerRpc(byte[] playerBytes)
    {
        PlayerData player = Deserialize<PlayerData>(playerBytes);
        AddPlayerLocal(player);
    }
}