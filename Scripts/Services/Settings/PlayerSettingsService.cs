using System;
using Godot;

namespace Fantoria.Scripts.Services.Settings;

[Service]
public class PlayerSettingsService
{
    
    private string _nick = "TestNick-" + Random.Shared.Next();
    private Color _color = new Color(1, 1, 1);

    public PlayerSettings GetPlayerSettings()
    {
        return new PlayerSettings(_nick, _color);
    }
}