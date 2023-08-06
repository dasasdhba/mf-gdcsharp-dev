using Entity;
using Godot;
using GlobalClass;

namespace Utils;

/// <summary>
/// Useful functions for getting entity or root with SceneTree Node by metadata.
/// </summary>
public static partial class Meta
{

    /// <summary>
    /// Get binded entity of the node.
    /// </summary>
    public static Entity2D GetEntity(Node node) => ((EntityContainer)node.GetMeta("Entity"))?.GetEntity();

    /// <summary>
    /// Get Root of binded entity of the node.
    /// </summary>
    public static Node GetRoot(Node node) => (GetEntity(node))?.Root;
}