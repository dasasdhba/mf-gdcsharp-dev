using Godot;

namespace Entity;

/// <summary>
/// Base entity class that holds an <c>Area2D</c> root node.
/// </summary>
public partial class Area : Entity2D
{
    /// <summary>
    /// <c>Area2D</c> root node.
    /// </summary>
    public virtual Area2D AreaNode { get; set; } = new();

    /// <summary>
    /// Collision shape of <c>Area2D</c> node.
    /// </summary>
    public virtual CollisionShape2D Shape { get; set; } = new()
    {
        Shape = new RectangleShape2D() { Size = new Vector2(32f, 32f) }
    };

    protected override void EntityFree() => AreaNode.QueueFree();

    protected override void SetComponents() 
    {
        Bind(AreaNode, true);
    }

    protected override void EnterTree(Node parent)
    {
        parent.CallDeferred("add_child", AreaNode);
        AreaNode.CallDeferred("add_child", Shape);
    }
}