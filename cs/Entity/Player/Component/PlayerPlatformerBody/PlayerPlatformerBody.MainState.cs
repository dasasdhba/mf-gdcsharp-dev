﻿using Component;
using System;
using Godot;

namespace Entity.Player;

public partial class PlayerPlatformerBody : PlatformerBody2D
{
    /// <summary>
    /// Whether the player is crouch, can not be set from external class.
    /// </summary>
    public bool Crouch { get; protected set; } = false;

    /// <summary>
    /// The max height that allows player to walk on the floor
    /// when colliding with a wall and near the floor.
    /// </summary>
    public float OnWallFixLength { get; set; } = 8f;
    private static readonly float OnWallFixSafeMargin = 4f;

    public StateMachine<PlayerPlatformerBody> MainStateMachine { get; set; }
        = new(MainState);

    /// <summary>
    /// The main player state that process general behaviors.
    /// </summary>
    public static readonly State<PlayerPlatformerBody> MainState = new()
    {
        Process = (PlayerPlatformerBody obj, double delta) => (
            obj.MainProcess(delta)
        ),

        Enter = (PlayerPlatformerBody obj, State<PlayerPlatformerBody> _) => {
            obj.JumpSpeedInit();
        }
    };

    public State<PlayerPlatformerBody> MainProcess(double delta)
    {
        // process walk

        int moveDir = Convert.ToInt32(MoveActions.Right.Pressed) - Convert.ToInt32(MoveActions.Left.Pressed);
        bool run = Actions["Fire"].Pressed;

        Crouch = GlobalData.PlayerState > 0 && MoveActions.Down.Pressed && IsOnFloor();
        
        // remake behavior of original Mario Forever
        // it looks strange but that's it

        if (!Crouch) { ProcessWalkSpeed(moveDir, run, delta); }
        else { ProcessCrouchSpeed(delta); }
        ProcessStopSpeed(moveDir, run, delta);

        SetWalkSpeed();

        // process jump

        if (!Crouch)
        {
            bool jumpInput = GetJumpInput();
            if (!IsInWater) { ProcessJump(jumpInput, delta); }
            else { ProcessSwim(jumpInput, delta); }
        }

        // process motion

        OnChangeShape(GlobalData.PlayerState, Crouch);

        // TODO: Checkup shape change and go to stuck state if necessary

        ProcessMotion(delta);

        // on wall fix

        if (GravitySpeed >= 0f && OnWallFixLength > 0f && IsOnWall())
        {
            Transform2D testTransform = GlobalTransform;
            testTransform.Origin += OnWallFixLength * UpDirection;
            Vector2 testMotion = OnWallFixSafeMargin * MFSpeedParam.Direction * -UpDirection.Orthogonal();
            if (!TestMove(testTransform, testMotion))
            {
                SetWalkSpeed();
                GlobalPosition += (float)(WalkParam.GetSpeed() * delta) * -UpDirection.Orthogonal();
                GlobalPosition += OnWallFixLength * UpDirection;
                MoveAndCollide((OnWallFixLength + 1) * -UpDirection);
            }
        }

        SetMFSpeed();

        return null;
    }

}