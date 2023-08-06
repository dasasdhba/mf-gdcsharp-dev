namespace Entity.Player;

/// <summary>
/// control will alwasy be false.
/// useful to ignore player control.
/// </summary>
public partial class PlayerInputPlatformerNull : PlayerInputPlatformer
{

    public override bool IsPressed(string key) => false;
    public override bool IsJustPressed(string key) => false;
    public override bool IsJustReleased(string key) => false;
}