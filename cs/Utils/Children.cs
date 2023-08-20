using Godot;

namespace Utils;

/// <summary>
/// Useful functions for children operation.
/// </summary>
public static partial class Children
{
    /// <summary>
    /// Copy children nodes as target node's children.
    /// Make sure the child has a parameterless constructor.
    /// Signal and group will be ignored.
    /// </summary>
    public static void CopyTo(Node source, Node target)
    {
        foreach (Node node in source.GetChildren())
        {
            Node duplicate = node.Duplicate((int)Node.DuplicateFlags.Scripts);
            target.AddChild(duplicate);
        }
    }

    /// <summary>
    /// Reparent children nodes as target node's children.
    /// </summary>
    public static void Reparent(Node source, Node target)
    {
        foreach (Node node in source.GetChildren())
        {
            node.Reparent(target);
        }
    }
}