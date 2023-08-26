using Godot;

namespace Entity.Fluid;

/// <summary>
/// Base water entity.
/// </summary>
public partial class Water : Area
{
    public override Area2D AreaNode { get; set; } = new AreaWater();
}