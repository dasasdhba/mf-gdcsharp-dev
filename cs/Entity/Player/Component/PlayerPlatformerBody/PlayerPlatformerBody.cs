using Component;
using GlobalClass;
using Utils;
using System.Collections.Generic;
using Game.Const;

namespace Entity.Player;

/// <summary>
/// Player platformer movement component.
/// Used by PlayerPlatformer.
/// </summary>
public partial class PlayerPlatformerBody : PlatformerBody2D
{
    public override AccelerationParam GravityParam { get; set; } = new(2500f, 2500f, 500f);

    // components
    public PlayerData GlobalData
    {
        get
        {
            if (BufferedData != null)
                return BufferedData;

            BufferedData = (Meta.GetEntity(this) as PlayerPlatformer)?.GlobalData;
            return BufferedData;
        }
        private set { }
    }

    private PlayerData BufferedData;

    public PlayerInputPlatformer InputHandle { get; set; } = new();

    protected OverlapObject2D WaterJumpDetector = new();
    public void WaterJumpDetectorAddShape(OverlappingShape2D shape) => 
        WaterJumpDetector.AddShape(shape);

    /// <summary>
    /// Direction input map with <c>UpDirection</c>.
    /// </summary>
    public PlayerInputPlatformer.DirInputMap MoveActions { get; set; }

    /// <summary>
    /// Input dict.
    /// </summary>
    public Dictionary<string, PlayerInput.ActionState> Actions { get; set; }

    public PlayerPlatformerBody() :base() 
    {
        CollisionLayer = Physics.PlayerLayer;
        CollisionMask = Physics.PlayerMask;

        WaterJumpDetector.QueryParameters.CollideWithAreas = true;
        WaterJumpDetector.QueryParameters.CollideWithBodies = false;

        TreeEntered += () =>
        {
            WaterJumpDetector.SetSpace(this);
            WaterJumpDetector.QueryParameters.CollisionMask = CollisionMask;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // update input map
        MoveActions = InputHandle.GetDirInputMap(UpDirection);
        Actions = InputHandle.GetInputMap();

        // process with state machine
        MainStateMachine.Process(this, delta);

    }

}