using GlobalClass;
using Godot;

namespace Entity.Player;

/// <summary>
/// Manage player collision shape.
/// </summary>
public partial class PlayerCollisionShape
{
    private static readonly float ShapeWidth = 19f;
    private static readonly float ShapeHeightSmall = 25f;
    private static readonly float ShapeHeightSuper = 52f;
    private static readonly float ShapeHeightWaterJump = 24f;

    public CollisionShape2D CollisionSmall { get; set; } = new()
    {
        Shape = new RectangleShape2D() { Size = new Vector2(ShapeWidth, ShapeHeightSmall) }
    };

    public CollisionShape2D CollisionSuper { get; set; } = new()
    {
        Shape = new RectangleShape2D() { Size = new Vector2(ShapeWidth, ShapeHeightSuper) },
        Position = new Vector2(0f, -(ShapeHeightSuper - ShapeHeightSmall) / 2f),
        Disabled = true
    };

    public OverlappingShape2D OverlappingWaterJump { get; set; } = new()
    {
        Shape = new RectangleShape2D() { Size = new Vector2(ShapeWidth, ShapeHeightWaterJump) },
        Position = new Vector2(0f, -(ShapeHeightSmall + ShapeHeightWaterJump) / 2)
    };

    /// <summary>
    /// Change player shape that should be called by event.
    /// </summary>
    /// <param name="state">1 or greater to super, 0 to small, -1 or less to disable.</param>
    public void OnChangeShape(int state)
    {
        CollisionSmall.Disabled = state != 0;
        CollisionSuper.Disabled = state <= 0;
        OverlappingWaterJump.Disabled = state < 0;

        float height = CollisionSuper.Disabled? ShapeHeightSmall : ShapeHeightSuper;
        OverlappingWaterJump.Position = new Vector2(0f, -(height + ShapeHeightWaterJump) / 2);
    }

    /// <summary>
    /// Should be called in <c>PlayerPlatformer.EnterTree(parent)</c>
    /// with the parent RootNode.
    /// </summary>
    public void EnterTree(PlayerPlatformerBody parent)
    {
        parent.CallDeferred("add_child", CollisionSmall);
        parent.CallDeferred("add_child", CollisionSuper);
        parent.CallDeferred("add_child", OverlappingWaterJump);
        parent.WaterJumpDetector.AddShape(OverlappingWaterJump);
    }
    
}