using Godot;

namespace Asset;

/// <summary>
/// Class <c> SpriteFrames </c> that holds SpriteFrames Resource.
/// A godot editor plugin has been implemented to auto generate holders 
/// for res://assets/sprite/
/// </summary>
public partial class SpriteFramesHolder
{
    protected string SpriteFramesPath;

    public SpriteFrames GetSpriteFrames()
    {
        if (GD.Load(SpriteFramesPath) is SpriteFrames spr) { return spr; }
        return null;
    }
}