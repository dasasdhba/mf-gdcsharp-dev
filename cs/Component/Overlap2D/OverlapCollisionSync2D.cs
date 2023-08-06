using Godot;
using System.Collections.Generic;

namespace Component;

/// <summary>
/// Overlap system that handles overlapping test.
/// Share the same CollisionShape2D with target CollisionObject2D.
/// </summary>
public partial class OverlapCollisionSync2D : OverlapManager2D
{

    public OverlapCollisionSync2D() : base() { }
    public OverlapCollisionSync2D(Rid Space) : base(Space) { }
    public OverlapCollisionSync2D(Node node) : base(node) { }

    /// <summary>
    /// The target CollisionObject2D to sync with.
    /// </summary>
    public CollisionObject2D SyncObject { get; set; }

    public override IEnumerable<ShapeInfo> GetShapeInfos()
    {
        if (GodotObject.IsInstanceValid(SyncObject))
        {
            foreach (int i in SyncObject.GetShapeOwners())
            {
                uint ui = (uint)i;
                if (SyncObject.IsShapeOwnerDisabled(ui)) { continue; }

                Node2D owner = (Node2D)SyncObject.ShapeOwnerGetOwner(ui);
                Transform2D transform = owner.GlobalTransform;

                for (int j = 0; j < SyncObject.ShapeOwnerGetShapeCount(ui); j++)
                {
                    yield return new ShapeInfo(
                        SyncObject.ShapeOwnerGetShape(ui,j),
                        transform
                        );
                }
            }
        }

        yield break;
    }
}