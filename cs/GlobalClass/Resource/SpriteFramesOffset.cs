using Godot;
using Godot.Collections;

namespace GlobalClass;

/// <summary>
/// SpriteFrames with offset setting for each frame.
/// </summary>
[GlobalClass]
public partial class SpriteFramesOffset : SpriteFrames
{
    /// <summary>
    /// Key: Animation name
    /// Item: SpriteFramesOffsetParam[]
    /// </summary>
    [ExportCategory("SpriteFramesOffset")]
    [Export]
    public Dictionary<string, SpriteFramesOffsetParam[]> Offsets { get; set; }
}