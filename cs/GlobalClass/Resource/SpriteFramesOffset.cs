using Godot;
using Godot.Collections;

namespace GlobalClass;

/// <summary>
/// SpriteFrames with offset setting for each frame.
/// </summary>
[GlobalClass]
public partial class SpriteFramesOffset : SpriteFrames
{
    [ExportCategory("SpriteFramesOffset")]

    /// <summary>
    /// Key: Animation name
    /// Item: SpriteFramesOffsetParam[]
    /// </summary>
    [Export]
    public Dictionary<string, SpriteFramesOffsetParam[]> Offsets { get; set; }
}