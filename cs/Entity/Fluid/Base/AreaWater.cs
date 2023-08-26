using Godot;
using Game.Const;

namespace Entity.Fluid;

/// <summary>
/// <c>Area2D</c> represents water.
/// </summary>
public partial class AreaWater : Area2D
{
    // water area based on Area2D
    public AreaWater() : base()
    {
        Monitorable = true;
        Monitoring = false;
        CollisionLayer = Physics.Obstacle;
        CollisionMask = 0;
    }
}