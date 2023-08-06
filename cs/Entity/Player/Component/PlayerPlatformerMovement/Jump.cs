using Entity.Fluid;
using GlobalClass;
using System;

namespace Entity.Player;

public partial class PlayerPlatformerMovement : PlatformerBody2D
{
    // jump and swim method

    public float JumpHeightMin
    {
        get => _JumpHeightMin;
        set
        {
            if (_JumpHeightMin != value)
            {
                _JumpHeightMin = value;
                JumpSpeed = (float)Math.Sqrt(2f * GravityParam.Acceleration * value);
            }
        }
    }

    private float _JumpHeightMin = 91f;

    public float JumpHeightIdle
    {
        get => _JumpHeightIdle;
        set
        {
            if (_JumpHeightIdle != value)
            {
                _JumpHeightIdle = value;
                JumpAccFixIdle = GravityParam.Acceleration -
                    ((GravityParam.Acceleration * JumpHeightMin) / value);
            }
        }
    }

    private float _JumpHeightIdle = 156.4f;

    public float JumpHeightMove
    {
        get => _JumpHeightMove;
        set
        {
            if (_JumpHeightMove != value)
            {
                _JumpHeightMove = value;
                JumpAccFixMove = GravityParam.Acceleration -
                    ((GravityParam.Acceleration * JumpHeightMin) / value);
            }
        }
    }

    private float _JumpHeightMove = 189f;

    public float JumpHeightLui
    {
        get => _JumpHeightLui;
        set
        {
            if (_JumpHeightLui != value)
            {
                _JumpHeightLui = value;
                JumpAccFixLui = GravityParam.Acceleration -
                    ((GravityParam.Acceleration * JumpHeightMin) / value);
            }
        }
    }

    private float _JumpHeightLui = 238f;

    public float JumpExtendWalkSpeed { get; set; } = 250f;

    // converted jump speed
    protected float JumpSpeed;

    protected float JumpAccFixIdle;
    protected float JumpAccFixMove;
    protected float JumpAccFixLui;

    public bool CanSwim { get; set; } = true;

    // swim speed is not worth handling with height parameter
    // since gravity param is likey to be changed when jump out water
    public float SwimSpeed { get; set; } = 155f;

    public float JumpOutWaterSpeed { get; set; } = 455f;

    /// <summary>
    /// Init jump speed with jumping height parameter.
    /// </summary>
    public void JumpSpeedInit()
    {
        JumpSpeed = (float)Math.Sqrt(2f * GravityParam.Acceleration * JumpHeightMin);
        Func<float, float> accFix = new((float height) => GravityParam.Acceleration -
                    ((GravityParam.Acceleration * JumpHeightMin) / height));
        JumpAccFixIdle = accFix(JumpHeightIdle);
        JumpAccFixMove = accFix(JumpHeightMove);
        JumpAccFixLui = accFix(JumpHeightLui);
    }

    private bool JumpPressed = false;

    /// <summary>
    /// Get jump input, should be called once in every _PhysicsProcess() call.
    /// </summary>
    protected bool GetJumpInput()
    {
        bool KeyPressed = Actions["Jump"].Pressed;
        if (JumpPressed)
        {
            JumpPressed = KeyPressed;
            return false;
        }

        return KeyPressed;
    }

    // process method
    protected void ProcessJump(bool jumpInput, double delta)
    {
        if (IsOnFloor())
        {
            if (jumpInput)
            {
                JumpPressed = true;
                SetGravitySpeed(-JumpSpeed, delta);
                OnPlayerJump();
            }
        }
        else
        {
            if (GravitySpeed < 0f && Actions["Jump"].Pressed)
            {
                float accFix;
                if (GlobalData.PlayerState == PlayerData.State.Lui)
                {
                    accFix = JumpAccFixLui;
                }
                else if (MFSpeedParam.Speed > JumpExtendWalkSpeed)
                {
                    accFix = JumpAccFixMove;
                }
                else
                {
                    accFix = JumpAccFixIdle;
                }

                GravitySpeed -= accFix * (float)delta;
            }
        }
    }

    protected void ProcessSwim(bool jumpInput, double delta)
    {
        if (!CanSwim) { return; }

        if (jumpInput)
        {
            JumpPressed = true;

            if (WaterJumpDetector.IsOverlapping<AreaWater>(true))
            {
                SetGravitySpeed(-SwimSpeed, delta);
                OnPlayerSwim();
            }
            else
            {
                SetGravitySpeed(-JumpOutWaterSpeed, delta);
                OnPlayerJumpOutWater();
            }
        }
    }
}