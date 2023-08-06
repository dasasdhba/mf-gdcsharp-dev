using Godot;

namespace Asset;

/// <summary>
/// Class <c> Texture2DHolder </c> that holds Texture2D Resource.
/// A godot editor plugin has been implemented to auto generate holders 
/// for res://assets/texture/
/// </summary>
public partial class Texture2DHolder
{
    protected string TexturePath;

    public Texture2D GetTexture()
    {
        if (GD.Load(TexturePath) is Texture2D tex) { return tex; }
        return null;
    }
}