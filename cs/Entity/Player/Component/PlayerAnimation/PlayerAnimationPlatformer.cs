using Godot;
using Component;
using Asset;

namespace Entity.Player;

/// <summary>
/// Player platformer animated sprite management node.
/// </summary>
public partial class PlayerAnimationPlatformer : Node2D
{
    protected PlayerPlatformerBody Body;
    protected PlayerStateAnimation StateAnim;
    protected PlayerAnimationNode Animation;
    protected PlayerTransformAnimation TransformAnim;

    /// <summary>
    /// The PackedScene resource has to own the same node tree structure
    /// with <c>MarioAnimation</c>.
    /// </summary>
    public PlayerAnimationPlatformer(PlayerPlatformerBody body, PackedSceneHolder animScene) : base()
    {
        Body = body;

        Node node = animScene.GetPackedScene().Instantiate();
        node.ReplaceBy(this);
        node.QueueFree();
        StateAnim = GetNode<PlayerStateAnimation>("StateAnimation");
        Animation = GetNode<PlayerAnimationNode>("Animation");
        TransformAnim = GetNode<PlayerTransformAnimation>("TransformAnimation");

        StateAnim.State = Body.GlobalData.PlayerState;

        Body.PlayerSwim += () => 
        {
            StateAnim.Frame = 0;
            StateAnim.Play("Swim");
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // main state animation
        State<PlayerPlatformerBody> state = Body.MainStateMachine.State;
        if (state == PlayerPlatformerBody.MainState)
            ProcessMainAnimation();
    }

    protected void ProcessMainAnimation()
    {
        StateAnim.FlipH = Body.WalkParam.Direction == -1;

        if (Body.IsOnFloor())
        {
            if (Body.Crouch)
            {
                StateAnim.Play("Crouch");
                return;
            }

            if (Body.MFSpeedParam.Speed <= Body.MFInitSpeed)
                StateAnim.Play("Idle");
            else
            {
                float scale = Body.MFSpeedParam.Speed / Body.MFRunParam.MaxSpeed;
                StateAnim.Play("Walk", 0.5f + scale * 1.2f);
            }

            return;
        }
        else
        {
            if (!Body.IsInWater)
                StateAnim.Play("Jump");
            else
            {
                if (StateAnim.Animation == "Swim" && StateAnim.GetCurrentSprite().IsPlaying())
                    return;

                StateAnim.Play("Dive");
            }
        }
    }
}