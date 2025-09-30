using Godot;

namespace Fantoria.Scenes.Screen.MainMenu.Pages.CreateServer;

public partial class MainMenuHostPage : MainMenuPage
{
    
    [Export] [NotNull] public TextEdit PortTextEdit { get; private set; }
    [Export] [NotNull] public TextEdit SaveNameTextEdit { get; private set; }
    [Export] [NotNull] public CheckBox IsDedicatedCheckBox { get; private set; }
    [Export] [NotNull] public Button CreateServerButton { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);

        CreateServerButton.Pressed += ParseAndStartServer;
    }

    private void ParseAndStartServer()
    {
        int? port = PortTextEdit.Text.Length != 0 ? PortTextEdit.Text.ToInt() : null;
        string saveFileName = SaveNameTextEdit.Text.Length != 0 ? SaveNameTextEdit.Text : null;
        bool isDedicated = IsDedicatedCheckBox.ButtonPressed;
        Service.MainScene.HostMultiplayerGameAsClient(port, saveFileName, isDedicated);
    }
}