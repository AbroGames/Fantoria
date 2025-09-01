using Godot;

namespace Fantoria.Scenes.Screen.MainMenu.Pages.ConnectToServer;

public partial class MainMenuConnectPage : MainMenuPage
{
    
    [Export] [NotNull] public TextEdit HostTextEdit { get; private set; }
    [Export] [NotNull] public TextEdit PortTextEdit { get; private set; }
    [Export] [NotNull] public Button ConnectToServerButton { get; private set; }
    
    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);

        ConnectToServerButton.Pressed += ParseAndConnectToServer;
    }

    private void ParseAndConnectToServer()
    {
        string host = PortTextEdit.Text.Length != 0 ? HostTextEdit.Text : null;
        int? port = PortTextEdit.Text.Length != 0 ? PortTextEdit.Text.ToInt() : null;
        Service.MainScene.ConnectToMultiplayerGame(host, port);
    }
}