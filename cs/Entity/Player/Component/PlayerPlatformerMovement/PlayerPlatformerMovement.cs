using Component;
using GlobalClass;
using System.Collections.Generic;

namespace Entity.Player;

/// <summary>
/// Player platformer movement component.
/// Used by PlayerPlatformer.
/// </summary>
public partial class PlayerPlatformerMovement : PlatformerBody2D
{

    // dependency components
    public PlayerData GlobalData { get; set; }
    public PlayerInputPlatformer InputHandle { get; set; } = new PlayerInputPlatformerNull();
    public OverlapObject2D WaterJumpDetector { get; set; }

    /// <summary>
    /// Direction input map with <c>UpDirection</c>.
    /// </summary>
    public PlayerInputPlatformer.DirInputMap MoveActions { get; set; }

    /// <summary>
    /// Input dict.
    /// </summary>
    public Dictionary<string, PlayerInput.ActionState> Actions { get; set; }

    public PlayerPlatformerMovement() :base() { }

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