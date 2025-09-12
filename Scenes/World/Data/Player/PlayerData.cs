using CommunityToolkit.Mvvm.ComponentModel;
using MessagePack;

namespace Fantoria.Scenes.World.Data.Player;

[MessagePackObject(AllowPrivate = true)]
public partial class PlayerData : ObservableObject
{
    
    [Key(0)] [ObservableProperty] private string _nick;
    
}