using Godot;
using Component;
using Asset.Scene;

namespace Entity.Player;

/// <summary>
/// Player Mario
/// </summary>
public partial class PlayerPlatformerMario : PlayerPlatformer, ISprite2D
{
    public Node2D Sprite { get; set; }

    public PlayerPlatformerMario() : base()
    {
        GlobalData.PlayerName = "Mario";
    }

    protected override void SetComponents()
    {
        base.SetComponents();

        Sprite = new PlayerAnimationPlatformer(RootNode, new MarioAnimation());
    }

    protected override void EnterTree(Node parent)
    {
        base.EnterTree(parent);

        RootNode.CallDeferred("add_child", Sprite);
    }
}