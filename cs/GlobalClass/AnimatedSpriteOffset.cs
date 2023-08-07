using Godot;
using Godot.Collections;

namespace GlobalClass;

/// <summary>
/// AnimatedSprite2D with offset setting.
/// </summary>
[GlobalClass]
public partial class AnimatedSpriteOffset :AnimatedSprite2D
{
    private Vector2 BaseOffset;

    public AnimatedSpriteOffset() : base()
    {
        TreeEntered += () => BaseOffset = Offset;
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