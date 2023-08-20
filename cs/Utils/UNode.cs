using Godot;

namespace Utils;

/// <summary>
/// Useful functions for node operation.
/// </summary>
public static partial class UNode
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
}