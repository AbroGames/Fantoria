using Godot;

namespace Fantoria.Scenes.Screen.LoadingScreen;

public partial class LoadingScreen : CanvasLayer
{
    [Export] [NotNull] public LoadingAnimHandle LoadingAnimHandle { get; private set; }
    [Export] [NotNull] public Label LoadingLabel { get; private set; }

    public override void _Ready()
    {
        NotNullChecker.CheckProperties(this);
    }

    public void SetText(string loadingText)
    {
        LoadingLabel.Text = loadingText.ToUpper();
    }
}
