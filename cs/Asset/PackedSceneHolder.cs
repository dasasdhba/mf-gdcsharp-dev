using Godot;

namespace Asset;

/// <summary>
/// Class <c> PackedSceneHolder </c> that PackedScene Resource.
/// A godot editor plugin has been implemented to auto generate holders 
/// for res://assets/scene/
/// </summary>
public partial class PackedSceneHolder
{
    protected string PackedScenePath;

    public PackedScene GetPackedScene()
    {
        if (GD.Load(PackedScenePath) is PackedScene scene) { return scene; }
        return null;
    }
}