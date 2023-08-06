using Godot;

namespace Entity.Player;

/// <summary>
/// Player Mario
/// </summary>
public partial class PlayerPlatformerMario : PlayerPlatformer
{

    public PlayerPlatformerMario()
    {
        GlobalData.PlayerName = "Mario";
    }

    // debug sprite
    protected override void EnterTree(Node parent)
    {
        base.EnterTree(parent);

        Sprite2D spr = new() { Texture = (Texture2D)GD.Load("res://main/icon.svg") };
        Root.CallDeferred("add_child", spr);
    }
}