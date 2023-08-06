using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Component;

/// <summary>
/// Base class of overlapping system.
/// Do overlapping query thourgh Godot Physics Server 2D.
/// </summary>
public partial class Overlap2D
{
    public PhysicsShapeQueryParameters2D QueryParameters { get; set; } = new() { CollisionMask = 32 };
    public int MaxResults { get; set; } = 32;

    // space
    private Rid Space = new();
    private PhysicsDirectSpaceState2D SpaceState;

    public Overlap2D() { }

    /// <summary>
    /// Construct with rid.
    /// </summary>
    /// <param name="space">The RID of space.</param>
    public Overlap2D(Rid space) => SetSpace(space);

    /// <summary>
    /// Construct with node, using the node's space as the physics space.
    /// Should be called at least in <c>Node._EnterTree()</c>,
    /// or <c>Node.GetViewport()</c> will return null.
    /// </summary>
    /// <param name="node">The node to query space.</param>
    public Overlap2D(Node node) => SetSpace(node);

    /// <summary>
    /// Change space.
    /// </summary>
    /// <param name="space">The RID of space.</param>
    public void SetSpace(Rid space)
    {
        Space = space;
        SpaceState = PhysicsServer2D.SpaceGetDirectState(Space);
    }

    /// <summary>
    /// Using the node's space as the physics space.
    /// Should be called at least in <c>Node._EnterTree()</c>,
    /// or <c>Node.GetViewport()</c> will return null.
    /// </summary>
    /// <param name="node">The node to query space.</param>
    public void SetSpace(Node node)
    {
        Rid space;
        if (node is Area2D area)
        {
            space = PhysicsServer2D.AreaGetSpace(area.GetRid());
        }
        else if (node is PhysicsBody2D body)
        {
            space = PhysicsServer2D.BodyGetSpace(body.GetRid());
        }
        else
        {
            space = node.GetViewport().FindWorld2D().Space;
        }

        SetSpace(space);
    }

    // exception
    public void ClearException() => QueryParameters.Exclude = new();
    public void AddException(Rid rid) => QueryParameters.Exclude.Add(rid);
    public void AddException(CollisionObject2D obj) => AddException(obj.GetRid());
    public void RemoveException(Rid rid) => QueryParameters.Exclude.Remove(rid);
    public void RemoveException(CollisionObject2D obj) => RemoveException(obj.GetRid());

    /// <summary>
    /// Overlapping query.
    /// </summary>
    /// <returns>an IEnumerable, please consider using a <c>foreach</c> statement.</returns>
    protected IEnumerable<GodotObject> QueryOverlappingObjects()
    {
        if (!PhysicsServer2D.SpaceIsActive(Space)) { yield break; }

        Array<Dictionary> query = SpaceState.IntersectShape(QueryParameters, MaxResults);
        foreach (Dictionary dict in query)
        {
            yield return (GodotObject)dict["collider"];
        }

        yield break;
    }

    /// <summary>
    /// Overlapping query with specific Class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    /// <returns>an IEnumerable, please consider using a <c>foreach</c> statement.</returns>
    protected IEnumerable<T> QueryOverlappingObjects<T>(bool excludeOthers = false) where T : GodotObject
    {
        if (!PhysicsServer2D.SpaceIsActive(Space)) { yield break; }

        Array<Dictionary> query = SpaceState.IntersectShape(QueryParameters, MaxResults);
        foreach (Dictionary dict in query)
        {
            GodotObject col = (GodotObject)dict["collider"];
            if (col is T colt) { yield return colt; }
            else if (excludeOthers) { AddException((CollisionObject2D)col); }
        }

        yield break;
    }
}