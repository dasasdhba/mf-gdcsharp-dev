﻿using Entity.Fluid;
using Godot;
using GlobalClass;

namespace Component;

/// <summary>
/// 2D Platformer based on CharacterBody2D.
/// Gravity direction is based on UpDirection.
/// </summary>
public partial class PlatformerBody2D : CharacterBody2D
{
    public float GravitySpeed { get; set; } = 0f;
    public virtual AccelerationParam GravityParam { get; set; } = new();
    public virtual AccelerationParam GravityParamWater { get; set; } = new(250f, 2125f, 150f);
    public virtual SpeedParam WalkParam { get; set; } = new();
    public bool GravityEnable { get; set; } = true;
    public bool WalkEnable { get; set; } = true;

    // IsInWater handled by WaterDetector in GravityProcess
    public bool IsInWater { get; protected set; } = false;

    // water detect
    protected OverlapCollisionSync2D WaterDetector;

    public PlatformerBody2D() : base()
    {
        WaterDetector = new() { SyncObject = this };
        WaterDetector.QueryParameters.CollideWithAreas = true;
        WaterDetector.QueryParameters.CollideWithBodies = false;

        TreeEntered += () =>
        {
            WaterDetector.QueryParameters.CollisionMask = CollisionMask;
            WaterDetector.SetSpace(this);
        };
    }

    /// <summary>
    /// Set gravity with acceleration fix.
    /// </summary>
    /// <param name="speed">The new gravity speed.</param>
    /// <param name="delta">delta of _PhysicsProcess call.</param>
    public void SetGravitySpeed(float speed, double delta)
    {
        AccelerationParam gravityParam = IsInWater ? GravityParamWater : GravityParam;
        GravitySpeed = speed - gravityParam.Acceleration * (float)delta / 2;
    }

    /// <summary>
    /// process gravity acceleration
    /// </summary>
    private void ProcessGravity(double delta)
    {
        if (!GravityEnable) return;

        IsInWater = WaterDetector.IsOverlapping<AreaWater>(true);

        AccelerationParam gravityParam = IsInWater ? GravityParamWater : GravityParam;
        GravitySpeed = gravityParam.ProcessSpeed(GravitySpeed, delta);
    }

    /// <summary>
    /// Process motion, if backport is enabled, GravitySpeed/WalkSpeed will be reset.
    /// </summary>
    /// <param name="backportGravity">Whether to backport velocity to gravity speed.</param>
    /// <param name="backportWalk">Whether to backport velocity to walk speed.</param>
    protected void ProcessMotion(double delta, bool backportGravity = true, bool backportWalk = true)
    {
        ProcessGravity(delta);

        Velocity = new Vector2(0f, 0f);

        if (GravityEnable) { Velocity -= UpDirection * GravitySpeed; }
        if (WalkEnable) { Velocity -= UpDirection.Orthogonal() * WalkParam.GetSpeed(); }

        if (MoveAndSlide())
        {
            if (backportGravity && GravityEnable) { 
                GravitySpeed = Velocity.Dot(-UpDirection); 
            }

            if (backportWalk && WalkEnable) { 
                WalkParam.SetSpeed(Velocity.Dot(-UpDirection.Orthogonal())); 
            }
        }
        
    }
}
