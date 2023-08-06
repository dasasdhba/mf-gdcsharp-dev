﻿using Godot;

namespace GlobalClass;

/// <summary>
/// Accleration Parameter Resource.
/// Useful for uniformly accelerated linear motion.
/// </summary>
[GlobalClass]
public partial class AccelerationParam : Resource
{
    // the default argument is used by MF general enemies 

    [ExportCategory("AccelerationParameter")]

    [Export]
    public float Acceleration { get; set; } = 1000f;

    [Export]
    public float Deceleration { get; set; } = 1000f;

    [Export]
    public float MaxSpeed { get; set; } = 500f;

    public AccelerationParam() { }
    public AccelerationParam(float acc, float dec, float maxSpeed)
    {
        Acceleration = acc;
        Deceleration = dec;
        MaxSpeed = maxSpeed;
    }

    /// <summary>
    /// Process speed with param and return a new speed.
    /// </summary>
    public float ProcessSpeed(float speed, double delta)
    {
        if (speed < MaxSpeed) 
        {
            speed += (float)(Acceleration * delta);
            if (speed >= MaxSpeed) { return MaxSpeed; }
        }

        if (speed > MaxSpeed) 
        {
            speed -= (float)(Deceleration * delta);
            if (speed <= MaxSpeed) { return MaxSpeed; }
        }

        return speed;
    }

    /// <summary>
    /// Process with SpeedParam.
    /// </summary>
    public void ProcessSpeed(SpeedParam param, double delta)
    {
        param.Speed = ProcessSpeed(param.Speed, delta);
    }
}