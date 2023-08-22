using Godot;

namespace Entity.Player;

/// <summary>
/// Player AnimationPlayer Node.
/// </summary>
public partial class PlayerAnimationNode : AnimationPlayer
{
    /// <summary>
    /// The PlayerStateAnimation Node.
    /// </summary>
    [ExportCategory("PlayerAnimationNode")]
    [Export]
    public PlayerStateAnimation StateAnimation { get; set; }

    public void PlaySpin() => Play("Spin");
    public void PlayClimb() => Play("Climb");

    /// <summary>
    /// Clear the animation settings.
    /// </summary>
    public virtual void Clear()
    {
        Stop();
        StateAnimation.Scale = new Vector2(1f, 1f);
    }
}