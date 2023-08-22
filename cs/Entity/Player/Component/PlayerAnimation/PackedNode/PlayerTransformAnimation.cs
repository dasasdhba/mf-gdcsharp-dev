using Godot;

namespace Entity.Player;

/// <summary>
/// Player AnimationPlayer Node
/// </summary>
public partial class PlayerTransformAnimation : PlayerAnimationNode
{
    /// <summary>
    /// The PlayerStateAnimation Node.
    /// </summary>
    [ExportCategory("PlayerTransformAnimation")]
    [Export]
    public PlayerTransformDrawer TransformDrawer { get; set; }

    // change state
    protected PlayerData.State LastState;
    protected PlayerData.State NewState;

    public void ChangeState(PlayerData.State newState)
    {
        LastState = StateAnimation.State;
        NewState = newState;
        if (LastState == PlayerData.State.Small || NewState == PlayerData.State.Small)
            Play("TransformSmall");
        else
            Play("Transform");
    }

    // used by animation player
    public void ChangeToLast() => StateAnimation.State = LastState;
    public void ChangeToNew() => StateAnimation.State = NewState;

    public override void Clear()
    {
        Stop();
        TransformDrawer.Hide();
        StateAnimation.Show();
    }
}