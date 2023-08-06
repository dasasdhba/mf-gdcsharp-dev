namespace Entity.Fluid;

/// <summary>
/// Base water entity.
/// </summary>
public partial class Water : Area
{
    public Water() => AreaNode = new AreaWater();
}