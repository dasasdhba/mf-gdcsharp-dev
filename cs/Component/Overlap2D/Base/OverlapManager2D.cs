using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Component;

/// <summary>
/// Base class of overlapping system with shape management.
/// Shape management needs to override to implement.
/// </summary>
public abstract partial class OverlapManager2D : Overlap2D
{
    /// <summary>
    /// Shape2D and its (Global) Transform used to query.
    /// </summary>
    public struct ShapeInfo
    {
        public Shape2D Shape;
        public Transform2D Transform;

        public ShapeInfo(Shape2D shape, Transform2D transform) => (Shape, Transform) = (shape, transform);
    }

    public OverlapManager2D() : base() { }
    public OverlapManager2D(Rid Space) : base(Space) { }
    public OverlapManager2D(Node node) : base(node) { }

    /// <summary>
    /// The manager will call this method to do overlapping query.
    /// Override to implement.
    /// </summary>
    public abstract IEnumerable<ShapeInfo> GetShapeInfos();

    private void SetShapeInfo(ShapeInfo info)
    {
        QueryParameters.Shape = info.Shape;
        QueryParameters.Transform = info.Transform;
    }

    private void SetShapeInfo(ShapeInfo info, Vector2 deltaPos)
    {
        QueryParameters.Shape = info.Shape;
        QueryParameters.Transform = new(
                info.Transform.X,
                info.Transform.Y,
                info.Transform.Origin + deltaPos
                );
    }

    /// <summary>
    /// Get overlapping query results.
    /// </summary>
    public GodotObject[] GetOverlappingObjects()
    {
        Dictionary<GodotObject, bool> hash = new();
        foreach (ShapeInfo info in GetShapeInfos()) 
        {
            SetShapeInfo(info);
            foreach (GodotObject obj in QueryOverlappingObjects())
            {
                hash[obj] = true;
            }
        }

        return hash.Keys.ToArray();
    }

    /// <summary>
    /// Get overlapping query results with a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public T[] GetOverlappingObjects<T>(bool excludeOthers = false) where T : GodotObject
    {
        Dictionary<T, bool> hash = new();
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info);
            foreach (T obj in QueryOverlappingObjects<T>(excludeOthers))
            {
                hash[obj] = true;
            }
        }

        return hash.Keys.ToArray();
    }

    /// <summary>
    /// Get overlapping results with delta pos.
    /// </summary>
    public GodotObject[] GetOverlappingObjects(Vector2 deltaPos)
    {
        Dictionary<GodotObject, bool> hash = new();
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info, deltaPos);
            foreach (GodotObject obj in QueryOverlappingObjects())
            {
                hash[obj] = true;
            }
        }

        return hash.Keys.ToArray();
    }

    /// <summary>
    /// Get overlapping query results with detla pos and a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public T[] GetOverlappingObjects<T>(Vector2 deltaPos, bool excludeOthers = false) where T : GodotObject
    {
        Dictionary<T, bool> hash = new();
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info, deltaPos);
            foreach (T obj in QueryOverlappingObjects<T>(excludeOthers))
            {
                hash[obj] = true;
            }
        }

        return hash.Keys.ToArray();
    }

    /// <summary>
    /// Check whether overlapping happens.
    /// </summary>
    public bool IsOverlapping()
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info);
            foreach (GodotObject _ in QueryOverlappingObjects())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether overlapping happens with a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public bool IsOverlapping<T>(bool excludeOthers = false) where T : GodotObject
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info);
            foreach (T _ in QueryOverlappingObjects<T>(excludeOthers))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether overlapping happens with a specific collision object.
    /// </summary>
    public bool IsOverlappingWith(GodotObject col)
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info);
            foreach (GodotObject obj in QueryOverlappingObjects())
            {
                if (obj == col) { return true; }
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos.
    /// </summary>
    public bool IsOverlapping(Vector2 deltaPos)
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info, deltaPos);
            foreach (GodotObject _ in QueryOverlappingObjects())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos and a specific class T.
    /// </summary>
    /// <param name="excludeOthers">Whether to set other class instance to exclusion list.</param>
    public bool IsOverlapping<T>(Vector2 deltaPos, bool excludeOthers = false) where T : GodotObject
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info, deltaPos);
            foreach (T _ in QueryOverlappingObjects<T>(excludeOthers))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether overlapping happens with delta pos and a specific collision object.
    /// </summary>
    public bool IsOverlappingWith(GodotObject col, Vector2 deltaPos)
    {
        foreach (ShapeInfo info in GetShapeInfos())
        {
            SetShapeInfo(info, deltaPos);
            foreach (GodotObject obj in QueryOverlappingObjects())
            {
                if (obj == col) { return true; }
            }
        }

        return false;
    }
}