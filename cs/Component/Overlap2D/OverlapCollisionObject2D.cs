using Godot;
using System.Collections.Generic;

namespace Component;

// There is no editor node implemented by this since it's not necessary.
// This class may be useful if you want to sync parts of the CollisionShape2D of an object.

/// <summary>
/// Overlap system that handles overlapping test.
/// Works with CollisionShape2D in the SceneTree.
/// </summary>
public partial class OverlapCollisionObject2D : OverlapManager2D
{

    public OverlapCollisionObject2D() : base() { }
    public OverlapCollisionObject2D(Rid Space) : base(Space) { }
    public OverlapCollisionObject2D(Node node) : base(node) { }

    public List<CollisionShape2D> OverlapShapes { get; set; } = new();

    public void AddShape(CollisionShape2D shape) => OverlapShapes.Add(shape);

    public void RemoveShape(CollisionShape2D shape) => OverlapShapes.Remove(shape);

    public override IEnumerable<ShapeInfo> GetShapeInfos()
    {
        foreach (CollisionShape2D shape in OverlapShapes)
        {
            if (!shape.Disabled) 
            { 
                yield return new ShapeInfo(shape.Shape,shape.GlobalTransform); 
            }
        }

        yield break;
    }
}