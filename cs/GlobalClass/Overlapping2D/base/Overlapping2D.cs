using Godot;
using Component;

namespace GlobalClass;

/// <summary>
/// Base class of OverlappingObjects.
/// Using Component.OverlapManager2D object as base API.
/// </summary>
[GlobalClass]
public abstract partial class Overlapping2D : Node2D
{

    /// <summary>
    /// The manager that needs to override.
    /// </summary>
    protected abstract OverlapManager2D GetOverlapManager();

    [ExportCategory("Overlapping2D")]

    [Export]
    public bool CollideWithAreas
    {
        get => _CollideWithAreas;
        set
        {
            _CollideWithAreas = value;
            GetOverlapManager().QueryParameters.CollideWithAreas = value;
        }
    }
    private bool _CollideWithAreas = false;

    [Export]
    public bool CollideWithBodies
    {
        get => _CollideWithBodies;
        set
        {
            _CollideWithBodies = value;
            GetOverlapManager().QueryParameters.CollideWithBodies = value;
        }
    }
    private bool _CollideWithBodies = true;

    [Export(PropertyHint.Range, "0,100,0.01")]
    public float Margin
    {
        get => _Margin;
        set
        {
            _Margin = value;
            GetOverlapManager().QueryParameters.Margin = value;
        }
    }
    private float _Margin = 0f;

    [Export]
    public int MaxResults
    {
        get => _MaxResults;
        set
        {
            _MaxResults = value;
            GetOverlapManager().MaxResults = value;
        }
    }
    private int _MaxResults = 32;

    [Export(PropertyHint.Layers2DPhysics)]
    public uint CollisionMask
    {
        get => _CollisionMask;
        set
        {
            _CollisionMask = value;
            GetOverlapManager().QueryParameters.CollisionMask = value;
        }
    }
    private uint _CollisionMask = 1;

    // init
    public Overlapping2D()
    {
        OverlapManager2D manager = GetOverlapManager();
        manager.QueryParameters.CollideWithAreas = CollideWithAreas;
        manager.QueryParameters.CollideWithBodies = CollideWithBodies;
        manager.QueryParameters.Margin = Margin;
        manager.MaxResults = MaxResults;
        manager.QueryParameters.CollisionMask = CollisionMask;

        TreeEntered += () => GetOverlapManager().SetSpace(this);
    }

    // exposed APIs

    // exception
    public void ClearException() => GetOverlapManager().ClearException();
    public void AddException(CollisionObject2D col) => GetOverlapManager().AddException(col);
    public void RemoveException(CollisionObject2D col) => GetOverlapManager().RemoveException(col);

    /// <summary>
    /// Get overlapping query results.
    /// </summary>
    public GodotObject[] GetOverlappingObjects()
    {
        return GetOverlapManager().GetOverlappingObjects();
    }

    /// <summary>
    /// Get overlapping query results with a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public T[] GetOverlappingObjects<T>(bool excludeOthers = false) where T : GodotObject
    {
        return GetOverlapManager().GetOverlappingObjects<T>(excludeOthers);
    }

    /// <summary>
    /// Get overlapping results with delta pos.
    /// </summary>
    public GodotObject[] GetOverlappingObjects(Vector2 deltaPos)
    {
        return GetOverlapManager().GetOverlappingObjects(deltaPos);
    }

    /// <summary>
    /// Get overlapping query results with detla pos and a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public T[] GetOverlappingObjects<T>(Vector2 deltaPos, bool excludeOthers = false) where T : GodotObject
    {
        return GetOverlapManager().GetOverlappingObjects<T>(deltaPos, excludeOthers);
    }

    /// <summary>
    /// Check whether overlapping happens.
    /// </summary>
    public bool IsOverlapping()
    {
        return GetOverlapManager().IsOverlapping();
    }

    /// <summary>
    /// Check whether overlapping happens with a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public bool IsOverlapping<T>(bool excludeOthers = false) where T : GodotObject
    {
        return GetOverlapManager().IsOverlapping<T>(excludeOthers);
    }

    /// <summary>
    /// Check whether overlapping happens with a specific collision object.
    /// </summary>
    public bool IsOverlappingWith(GodotObject col)
    {
        return GetOverlapManager().IsOverlappingWith(col);
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos.
    /// </summary>
    public bool IsOverlapping(Vector2 deltaPos)
    {
        return GetOverlapManager().IsOverlapping(deltaPos);
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos and a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public bool IsOverlapping<T>(Vector2 deltaPos, bool excludeOthers = false) where T : GodotObject
    {
        return GetOverlapManager().IsOverlapping<T>(deltaPos, excludeOthers);
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos and a specific collision object.
    /// </summary>
    public bool IsOverlappingWith(GodotObject col, Vector2 deltaPos)
    {
        return GetOverlapManager().IsOverlappingWith(col, deltaPos);
    }
}