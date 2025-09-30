﻿using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using MessagePack;

namespace Fantoria.Scenes.World.Data.MapPoint;

[MessagePackObject(AllowPrivate = true)]
public partial class MapPointData : ObservableObject
{
    
    [Key(0)] [ObservableProperty] private long _id;
    
    [Key(1)] [ObservableProperty] private float _positionX;
    [Key(2)] [ObservableProperty] private float _positionY;

}