using Godot;
using GlobalClass;
using Component;
using Asset;
using Asset.Audio;

namespace Entity.Player;

/// <summary>
/// Mario Forever base platformer entity.
/// </summary>
public partial class PlayerPlatformer : Player
{
	// Components
	public PlayerPlatformerBody RootNode { get; set; }
	public PlayerCollisionShape PlayerShape { get; set; } = new();

    // audio
    public AudioStreamManager Audio { get; set; }

	protected virtual void AudioInit()
	{
		Audio = new(RootNode, new AudioStreamHolder[]
				{
					new PlayerJump(),
					new PlayerStomp(),
					new PlayerSwim(),
					new PowerDown(),
					new PowerUp(),
					new StarmanRunningOut(),
					new PlayerDeath()
				}
			);

		RootNode.PlayerJump += () => Audio.Play<PlayerJump>();
		RootNode.PlayerSwim += () => Audio.Play<PlayerSwim>();
        RootNode.PlayerJumpOutWater += () => Audio.Play<PlayerSwim>();
    }

    protected override void EntityFree()
	{
		Audio.Free();
		RootNode.QueueFree();
	}

    protected override void SetComponents()
	{
		AccessGlobalData();

		// root node
		RootNode = new PlayerPlatformerBody()
        {
			Transform = Transform,
			GravityParam = new AccelerationParam(2500f, 2500f, 500f),
		};
		Bind(RootNode, true);

		// set up audio
		AudioInit();

        // event subscribe
        RootNode.ChangeShape += PlayerShape.OnChangeShape;
	}

    protected override void EnterTree(Node parent)
	{
		parent.CallDeferred("add_child", RootNode);
        PlayerShape.EnterTree(RootNode);
    }
}
