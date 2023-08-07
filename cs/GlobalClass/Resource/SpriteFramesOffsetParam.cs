using Godot;

namespace GlobalClass;

/// <summary>
/// SpriteFramesOffset Parameter.
/// </summary>
[GlobalClass]
public partial class SpriteFramesOffsetParam : Resource
{
    [ExportCategory("SpriteFramesOffsetParameter")]

    [Export]
    public int Frame { get; set; } = 0;

    [Export]
    public Vector2 Offset { get; set; } = new Vector2(0, 0);
}