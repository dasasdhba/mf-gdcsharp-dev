using Godot;
using System;

namespace Utils;

/// <summary>
/// Useful function to get current view info.
/// </summary>
public static partial class View
{

    private static ulong LastPhysicsFrame = 0;
    private static Viewport LastViewport;
    private static Rect2 LastViewRect;

    /// <summary>
    /// Return the same in same viewport and physics frame if forceUpdate is not enabled.
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="forceUpdate">Whether to use buffered result if available.</param>
    public static Rect2 GetViewRect(CanvasItem item, bool forceUpdate = false)
    {
        ulong physicsFrame = Engine.GetPhysicsFrames();
        Viewport viewport = item.GetViewport();

        if (!forceUpdate && physicsFrame == LastPhysicsFrame && viewport == LastViewport)
        {
            return LastViewRect;
        }

        LastPhysicsFrame = physicsFrame;
        LastViewport = viewport;

        Transform2D canvas = item.GetCanvasTransform();
        Vector2 topLeft = -canvas.Origin / canvas.Scale;
        Vector2 size = item.GetViewportRect().Size / canvas.Scale;

        Rect2 result = new(topLeft, size);
        LastViewRect = result;

        return result;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInView(Node2D item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Grow(eps).HasPoint(item.GlobalPosition);
    }

    /// <summary>
    /// Whether the CanvasItem is in current view
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInView(Control item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Grow(eps).HasPoint(item.GlobalPosition);
    }

    /// <summary>
    /// Whether the CanvasItem is in current view left
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewLeft(Node2D item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.X - eps <= item.GlobalPosition.X;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view left
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewLeft(Control item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.X - eps <= item.GlobalPosition.X;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view right
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewRight(Node2D item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.X + eps >= item.GlobalPosition.X;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view right
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewRight(Control item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.X + eps >= item.GlobalPosition.X;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view top
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewTop(Node2D item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.Y - eps <= item.GlobalPosition.Y;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view top
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewTop(Control item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.Y - eps <= item.GlobalPosition.Y;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view bottom
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewBottom(Node2D item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.Y + eps >= item.GlobalPosition.Y;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view bottom
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewBottom(Control item, float eps = 0f, bool forceUpdate = false)
    {
        return GetViewRect(item, forceUpdate).Position.Y + eps >= item.GlobalPosition.Y;
    }

    /// <summary>
    /// Whether the CanvasItem is in current view with specific direction.
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="dir">The direction to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewDir(Node2D item, Vector2 dir, float eps = 0f, bool forceUpdate = false)
    {
        if (Math.Abs(dir.Y) >= Math.Abs(dir.X))
        {
            return dir.Y >= 0 ? IsInViewBottom(item, eps, forceUpdate) : IsInViewTop(item, eps, forceUpdate);
        }

        return dir.X >= 0 ? IsInViewRight(item, eps, forceUpdate) : IsInViewLeft(item, eps, forceUpdate);
    }

    /// <summary>
    /// Whether the CanvasItem is in current view with specific direction.
    /// </summary>
    /// <param name="item">The CanvasItem to query.</param>
    /// <param name="dir">The direction to query.</param>
    /// <param name="eps">Set positive to extend judging view, or negative to reduce.</param>
    /// <param name="forceUpdate">Whether to use buffered view if available.</param>
    public static bool IsInViewDir(Control item, Vector2 dir, float eps = 0f, bool forceUpdate = false)
    {
        if (Math.Abs(dir.Y) >= Math.Abs(dir.X))
        {
            return dir.Y >= 0 ? IsInViewBottom(item, eps, forceUpdate) : IsInViewTop(item, eps, forceUpdate);
        }

        return dir.X >= 0 ? IsInViewRight(item, eps, forceUpdate) : IsInViewLeft(item, eps, forceUpdate);
    }

}