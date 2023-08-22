using Godot;

namespace Component;

/// <summary>
/// Interface for 2d sprite management node of Entity2D.
/// </summary>
public interface ISprite2D
{
    /// <summary>
    /// The sprite 2d manager node.
    /// Put and process the sprite nodes as children.
    /// </summary>
    public Node2D Sprite { get; set; }
}