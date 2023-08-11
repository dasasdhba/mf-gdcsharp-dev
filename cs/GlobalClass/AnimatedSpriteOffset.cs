using Godot;
using Godot.Collections;

namespace GlobalClass;

/// <summary>
/// AnimatedSprite2D with offset setting.
/// Please ignore AnimatedSprite's Offset property.
/// </summary>
[GlobalClass]
public partial class AnimatedSpriteOffset :AnimatedSprite2D
{
    [ExportCategory("AnimatedSpriteOffset")]

    [Export]
    public Vector2 BaseOffset { get; set; } = new Vector2(0, 0);

    public AnimatedSpriteOffset() : base()
    {
        AnimationChanged += SetOffset;
        FrameChanged += SetOffset;
        SpriteFramesChanged += SetOffset;
    }

    protected void SetOffset()
    {
        if (SpriteFrames is SpriteFramesOffset sprOffset)
        {
            Dictionary<string, SpriteFramesOffsetParam[]> offsetDict = sprOffset.Offsets;
            if (offsetDict.ContainsKey(Animation))
            {
                foreach (SpriteFramesOffsetParam param in offsetDict[Animation]) 
                {
                    if (Frame == param.Frame) 
                    { 
                        Offset = BaseOffset + param.Offset;
                        return;
                    }
                }
            }
        }

        Offset = BaseOffset;
    }
}