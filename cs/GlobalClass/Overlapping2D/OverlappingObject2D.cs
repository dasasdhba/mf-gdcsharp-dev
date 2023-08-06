using Component;
using Godot;

namespace GlobalClass;

/// <summary>
/// Overlap2D handles overlapping test.
/// using OverlappingShape2D child as shape.
/// </summary>
[GlobalClass]
public partial class OverlappingObject2D : Overlapping2D
{

    private OverlapObject2D OverlapObject = new();
    protected override OverlapManager2D GetOverlapManager() => OverlapObject;

    public OverlappingObject2D() : base()
    {
        ChildEnteredTree += AddShape;
        ChildExitingTree += RemoveShape;
    }

    // handle child shape
    protected void AddShape(Node node)
    {
        if (node is OverlappingShape2D shape)
        {
            OverlapObject.AddShape(shape);
        }
    }

    protected void RemoveShape(Node node)
    {
        if (node is OverlappingShape2D shape)
        {
            OverlapObject.RemoveShape(shape);
        }
    }

}