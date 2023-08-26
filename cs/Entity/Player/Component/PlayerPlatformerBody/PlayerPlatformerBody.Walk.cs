using Component;
using System;

namespace Entity.Player;

public partial class PlayerPlatformerBody : PlatformerBody2D
{
    // walk method

    /// <summary>
    /// Walk speed in CTF 8-direction unit.
    /// </summary>
    public SpeedParam MFSpeedParam { get; protected set; } = new();

    // walk parameter can not be accessed by external class directly for safety reason
    // consider using Set/GetMFWalkParam() instead
    public float MFInitSpeed { get; protected set; } = 400f;
    public AccelerationParam MFWalkParam { get; protected set; } = new AccelerationParam(2500f, 0f, 1750f);
    public AccelerationParam MFRunParam { get; protected set; } = new AccelerationParam(2500f, 2500f, 3000f);
    public AccelerationParam MFTurnParam { get; protected set; } = new AccelerationParam(0f, 7500f, 0f);
    public AccelerationParam MFStopParam { get; protected set; } = new AccelerationParam(0f, 2500f, 0f);
    public AccelerationParam MFRunStopParam { get; protected set; } = new AccelerationParam(0f, 2500f, 1750f);
    public AccelerationParam MFCrouchParam { get; protected set; } = new AccelerationParam(0f, 2500f, 0f);

    /// <summary>
    /// Help structure for getting/setting walk parameter.
    /// </summary>
    public struct MFWalkParameter
    {
        public float InitSpeed { get; set; } = 400f;
        public float WalkSpeed { get; set; } = 1750f;
        public float RunSpeed { get; set; } = 3000f;
        public float Acceleration { get; set; } = 2500f;
        public float Deceleration { get; set; } = 2500f;
        public float TurnDeceleration { get; set; } = 7500f;
        public float StopDeceleration { get; set; } = 2500f;
        public float RunStopDeceleration { get; set; } = 2500f;
        public float CrouchDeceleration { get; set; } = 2500f;

        public MFWalkParameter() { }
    }

    /// <summary>
    /// Change walk parameter with <c>MFWalkParameter</c>
    /// </summary>
    public void SetMFWalkParam(MFWalkParameter param)
    {
        MFInitSpeed = param.InitSpeed;
        MFWalkParam.Acceleration = param.Acceleration;
        MFWalkParam.MaxSpeed = param.WalkSpeed;
        MFRunParam.Acceleration = param.Acceleration;
        MFRunParam.Deceleration = param.Deceleration;
        MFRunParam.MaxSpeed = param.RunSpeed;
        MFTurnParam.Deceleration = param.TurnDeceleration;
        MFStopParam.Deceleration = param.StopDeceleration;
        MFRunStopParam.Deceleration = param.RunStopDeceleration;
        MFRunStopParam.MaxSpeed = param.WalkSpeed;
        MFCrouchParam.Deceleration = param.CrouchDeceleration;
    }

    /// <summary>
    /// Get current walk parameter as a <c>MFWalkParameter</c> struct.
    /// </summary>
    public MFWalkParameter GetMFWalkParam()
    {
        return new MFWalkParameter()
        {
            InitSpeed = MFInitSpeed,
            WalkSpeed = MFWalkParam.MaxSpeed,
            RunSpeed = MFRunParam.MaxSpeed,
            Acceleration = MFRunParam.Acceleration,
            Deceleration = MFRunParam.Deceleration,
            TurnDeceleration = MFTurnParam.Deceleration,
            StopDeceleration = MFStopParam.Deceleration,
            RunStopDeceleration = MFRunStopParam.Deceleration,
            CrouchDeceleration = MFCrouchParam.Deceleration
        };
    }

    /// <summary>
    /// Set walk speed with real param.
    /// </summary>
    public void SetRealWalkSpeed(float speed)
    {
        WalkParam.Speed = speed;
        SetMFSpeed();
    }

    /// <summary>
    /// Set walk speed with mf param.
    /// </summary>
    public void SetMFWalkSpeed(float speed)
    {
        MFSpeedParam.Speed = speed;
        SetWalkSpeed();
    }

    /// <summary>
    /// Set walk direction without reset speed.
    /// </summary>
    public void SetWalkDirection(int dir)
    {
        WalkParam.Direction = dir;
        MFSpeedParam.Direction = dir;
    }

    /// <summary>
    /// Change walk direction without reset speed.
    /// </summary>
    public void ChangeWalkDirection() => SetWalkDirection(MFSpeedParam.Direction * -1);

    /// <summary>
    /// Convert MF Speed Param to real one. See
    /// <see href="https://www.marioforever.net/thread-2734-1-1.html">Mario Forever Community</see>
    /// for more information.
    /// </summary>
    protected void SetWalkSpeed()
    {
        WalkParam.Speed = Math.Max(0f, (MFSpeedParam.Speed - 50f) / 8f);
        WalkParam.Direction = MFSpeedParam.Direction;
    }

    /// <summary>
    /// Convert real walk speed to MF Speed Param.
    /// </summary>
    /// <seealso cref="SetWalkSpeed"/>
    protected void SetMFSpeed()
    {
        MFSpeedParam.Speed = WalkParam.Speed * 8f + 50f;
        MFSpeedParam.Direction = WalkParam.Direction;
    }

    // process method
    protected void ProcessWalkSpeed(int moveDir, bool run, double delta)
    {
        // walk and run
        if (moveDir == MFSpeedParam.Direction)
        {
            if (MFSpeedParam.Speed < MFInitSpeed) { MFSpeedParam.Speed += MFInitSpeed; }
            else
            {
                AccelerationParam accParam = (run && !IsInWater) ? MFRunParam : MFWalkParam;
                accParam.ProcessSpeed(MFSpeedParam, delta);
            }
        }
        // turn
        else if (moveDir != 0)
        {
            MFTurnParam.ProcessSpeed(MFSpeedParam, delta);

            // change direction
            if (MFSpeedParam.Speed <= 0f) { MFSpeedParam.Direction *= -1; }
        }

    }

    protected void ProcessStopSpeed(int moveDir, bool run, double delta)
    {
        if (moveDir == 0) { MFStopParam.ProcessSpeed(MFSpeedParam, delta); }
        if (!run) { MFRunStopParam.ProcessSpeed(MFSpeedParam, delta); }
    }

    protected void ProcessCrouchSpeed(double delta)
    {
        MFCrouchParam.ProcessSpeed(MFSpeedParam, delta);
    }

    // clear
    protected void ClearWalkSpeed() => SetMFWalkSpeed(0f);
}