using Godot;

namespace Fantoria.Scenes.Screen.MainMenu.Pages.Main;

public partial class MainMenuMainPage : MainMenuPage
{
    
    [Export] [NotNull] public Button StartSingleplayerButton { get; private set; }
    [Export] [NotNull] public Button CreateServerButton { get; private set; }
    [Export] [NotNull] public Button ConnectToServerButton { get; private set; }
    [Export] [NotNull] public Button SettingsButton { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
        
        StartSingleplayerButton.Pressed += () => Service.MainScene.StartSingleplayerGame();
        CreateServerButton.Pressed += () => ChangeMenuPage(PackedScenes.CreateServer);
        ConnectToServerButton.Pressed += () => ChangeMenuPage(PackedScenes.ConnectToServer);
        SettingsButton.Pressed += () => ChangeMenuPage(PackedScenes.Settings);
    }
}