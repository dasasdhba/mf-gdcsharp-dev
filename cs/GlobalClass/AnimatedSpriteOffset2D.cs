using Godot;
using Godot.Collections;

namespace GlobalClass;

/// <summary>
/// AnimatedSprite2D with offset setting.
/// Please ignore AnimatedSprite's Offset property.
/// </summary>
[GlobalClass]
public partial class AnimatedSpriteOffset2D :AnimatedSprite2D
{
    [ExportCategory("AnimatedSpriteOffset2D")]

    [Export]
    public Vector2 BaseOffset
    {
        get => _BaseOffset;
        set
        {
            if (_BaseOffset != value)
            {
                _BaseOffset = value;
                SetOffset();
            }
        }
    }
    private Vector2 _BaseOffset = new(0f, 0f);

    public AnimatedSpriteOffset2D() : base()
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