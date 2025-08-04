using Fantoria.Lib.Services;
using Godot;

namespace Fantoria.Lib.Nodes.Camera.Shifts;

public class ManualShake : IShiftProvider
{
    /// <inheritdoc />
    public Vector2 Shift => IsAlive ? LibService.Rand.InsideUnitCircle * Strength : Vec();

    public float Strength { get; set; } = 0;

    /// <inheritdoc />
    public bool IsAlive { get; set; } = true;

    /// <inheritdoc />
    public void Update(double delta)
    {
        // do nothing
    }
}
