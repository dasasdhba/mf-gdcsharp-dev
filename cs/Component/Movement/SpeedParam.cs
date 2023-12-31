﻿using System;

namespace Component;

/// <summary>
/// Speed Paramater Resource.
/// Useful for linear motion with two directions.
/// </summary>
public partial class SpeedParam
{
    private int _direction = 1;

    /// <summary>
    /// Direction will be limited to 1 or -1, setting to 0 will be ignored.
    /// </summary>
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

    public SpeedParam() { }

    /// <summary>
    /// Get speed with direction.
    /// </summary>
    /// <param name="useDirection">Whether to use direction.</param>
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