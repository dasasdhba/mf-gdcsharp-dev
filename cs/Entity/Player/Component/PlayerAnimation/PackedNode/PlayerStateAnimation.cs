using Godot;
using GlobalClass;

namespace Entity.Player;

/// <summary>
/// Manage PlayerStateSprite Node.
/// </summary>
public partial class PlayerStateAnimation : UniformAnimatedSprite2D
{
    /// <summary>
    /// The current player animation state.
    /// </summary>
    [ExportCategory("PlayerStateAnimation")]
    [Export]
    public PlayerData.State State
    {
        get => _State;
        set
        {
            if (_State != value)
            {
                _State = value;
                UpdateState();
            }
        }
    }
    private PlayerData.State _State = PlayerData.State.Small;

    public PlayerStateAnimation() : base()
    {
        Animation = "Idle";

        ChildEnteredTree += (Node node) =>
        {
            if (node is PlayerStateSprite spr)
            {
                spr.Visible = spr.State == State;
            }
        };
    }

    /// <summary>
    /// Update current state animation.
    /// </summary>
    public void UpdateState()
    {
        foreach (AnimatedSprite2D spr in Sprites)
        {
            if (spr is PlayerStateSprite playerSpr)
            {
                playerSpr.Visible = playerSpr.State == State;
            }
        }
    }

    /// <summary>
    /// Get specific AnimatedSprite2D node.
    /// </summary>
    public PlayerStateSprite GetSprite(PlayerData.State state)
    {
        foreach (AnimatedSprite2D spr in Sprites)
        {
            if (spr is PlayerStateSprite playerSpr)
            {
                if (playerSpr.State == state)
                    return playerSpr;
            }
        }

        return null;
    }

    /// <summary>
    /// Get current AnimatedSprite2D node.
    /// </summary>
    public PlayerStateSprite GetCurrentSprite() => GetSprite(State);

}