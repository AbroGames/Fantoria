﻿using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using MessagePack;

namespace Fantoria.Scenes.World.Data.Player;

[MessagePackObject(AllowPrivate = true)]
public partial class PlayerData : ObservableObject
{
    
    [Key(0)] [ObservableProperty] private string _nick;
    
    [Key(1)] [ObservableProperty] private float _colorR;
    [Key(2)] [ObservableProperty] private float _colorG;
    [Key(3)] [ObservableProperty] private float _colorB;

    [IgnoreMember]
    public Color Color
    {
        get => new(ColorR, ColorG, ColorB);
        set
        {
            ColorR = value.R;
            ColorG = value.G;
            ColorB = value.B;
        }
    } 
}