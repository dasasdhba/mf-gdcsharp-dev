using System;
using Godot;

namespace GlobalClass;

/// <summary>
/// Speed Paramater With Acceleration settting.
/// Useful for uniformly accelerated linear motion with two directions.
/// </summary>
[GlobalClass]
public partial class SpeedAccParam : SpeedParam
{
    /// <summary>
    /// The direction to accelerate.
    /// </summary>
    [ExportCategory("SpeedAccParameter")]
    [Export]
    public int AccDirection
    {
        get { return _AccDirection; }
        set
        {
            if (value != 0)
            {
                _AccDirection = Math.Sign(value);
            }
        }
    }

    private int _AccDirection = 1;

    /// <summary>
    /// The Acceleration Parameter used for processing speed.
    /// The MaxSpeed has to be positive.
    /// </summary>
    [Export]
    public AccelerationParam AccParameter { get; set; }

    public SpeedAccParam() : base() => AccParameter ??= new(500f, 500f, 250f);

    public SpeedAccParam(float acceleration, float deceleration, float maxSpeed) :base() =>
        AccParameter = new(acceleration, deceleration, maxSpeed);

    /// <summary>
    /// Process <c>Speed</c> with SpeedAccParameters
    /// </summary>
    public void ProcessSpeed(double delta)
    {
        if (Direction == AccDirection)
        {
            AccParameter.ProcessSpeed(this, delta);
        }
        else
        {
            Speed -= (float)(AccParameter.Deceleration * delta);
        }
    }
}