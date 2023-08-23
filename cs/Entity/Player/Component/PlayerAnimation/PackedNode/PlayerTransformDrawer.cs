using GlobalClass;
using Godot;

namespace Entity.Player;

/// <summary>
/// Draw player transform animation.
/// </summary>
public partial class PlayerTransformDrawer : Draw2D
{
    /// <summary>
    /// The PlayerStateAnimation Node.
    /// </summary>
    [ExportCategory("PlayerTransformDrawer")]
    [Export]
    public PlayerStateAnimation StateAnimation { get; set; }

    private AnimatedSprite2D BufferedSprite;

    public override bool DrawProcess(double delta)
    {
        if (StateAnimation != null)
        {
            FlipH = StateAnimation.FlipH;
            AnimatedSprite2D spr = StateAnimation.GetCurrentSprite();
            if (BufferedSprite == spr)
                return false;
            BufferedSprite = spr;

            SpriteFrames frames = StateAnimation.GetCurrentSprite().SpriteFrames;

            SetMaterial(spr.Material);
            QueuedDrawSpriteFrames(frames, "Transform", 0, spr.Position);

            return true;
        }

        return false;
    }
}