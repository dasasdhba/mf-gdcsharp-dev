using Godot;

namespace Entity.Player;

/// <summary>
/// Player state AnimatedSprite2D node.
/// </summary>
public partial class PlayerStateSprite : AnimatedSprite2D
{
    [ExportCategory("PlayerAnimatedSprite")]
    [Export]
    public PlayerData.State State { get; set; } = PlayerData.State.Small;
}