using Godot;
using GlobalClass;
using System.Collections.Generic;

namespace Component;

/// <summary>
/// Overlap system that handles overlapping test.
/// Works with OverlappingShape2D in the SceneTree.
/// </summary>
public partial class OverlapObject2D : OverlapManager2D
{
    public OverlapObject2D() : base() { }
    public OverlapObject2D(Rid Space) : base(Space) { }
    public OverlapObject2D(Node node) : base(node) { }

    public List<OverlappingShape2D> OverlapShapes { get; set; } = new();

    public void AddShape(OverlappingShape2D shape) => OverlapShapes.Add(shape);

    public void RemoveShape(OverlappingShape2D shape) => OverlapShapes.Remove(shape);

    public override IEnumerable<ShapeInfo> GetShapeInfos()
    {
        foreach (OverlappingShape2D shape in OverlapShapes)
        {
            if (!shape.Disabled)
            {
                yield return new ShapeInfo(shape.Shape, shape.GlobalTransform);
            }
        }

        yield break;
    }
}