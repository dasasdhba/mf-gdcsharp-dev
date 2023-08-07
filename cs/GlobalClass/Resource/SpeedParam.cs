using System;
using Godot;

namespace GlobalClass;

/// <summary>
/// Speed Paramater Resource.
/// Useful for linear motion with two directions.
/// </summary>
[GlobalClass]
public partial class SpeedParam : Resource
{

    [ExportCategory("SpeedParameter")]

    private int _direction = 1;

    /// <summary>
    /// Direction will be limited to 1 or -1, setting to 0 will be ignored.
    /// </summary>
    [Export(PropertyHint.Enum, "Left:-1, Right:1")]
    public int Direction
    {
        get { return _direction; }
        set
        {
            if ( value != 0 )
            {
                _direction = Math.Sign(value);
            }
        }
    }

    private float _speed = 0f;

    /// <summary>
    /// Direction will be changed if setting this property to negative.
    /// </summary>
    [Export]
    public float Speed 
    { 
        get{ return _speed; }
        set
        {
            if ( value < 0f ) 
            {
                Direction *= -1;
                value *= -1f;
            }
            _speed = value;
        }
    }

    public SpeedParam() : base() { }

    /// <summary>
    /// Get speed with direction.
    /// </summary>
    /// <param name="useDirection">Whether to use direction.</param>
    /// <returns></returns>
    public float GetSpeed(bool useDirection = true)
    {
        return useDirection ? Speed * Direction : Speed;
    }

    /// <summary>
    /// Set speed with direction.
    /// </summary>
    public void SetSpeed(float speed)
    {
        _speed = Math.Abs(speed);
        if (speed != 0f) { _direction = Math.Sign(speed); }
    }
}