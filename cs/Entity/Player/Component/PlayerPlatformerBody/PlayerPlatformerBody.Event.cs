using System;
using GlobalClass;

namespace Entity.Player;
public partial class PlayerPlatformerBody : PlatformerBody2D
{
    // event of player platformer movement
    //
    // subscriber is set up by entity

    // change shape
    public event Action<int> ChangeShape;

    private int LastState = 0;
    protected virtual void OnChangeShape(int state)
    {
        if (state == LastState) { return; }
        LastState = state;

        ChangeShape?.Invoke(state);
    }
    protected virtual void OnChangeShape(PlayerData.State state, bool crouch)
    {
        if (state > 0 && crouch) { OnChangeShape(0); }
        else { OnChangeShape((int)state); }
    }

    // jump
    public event Action PlayerJump;
    protected virtual void OnPlayerJump() => PlayerJump?.Invoke();

    // swim
    public event Action PlayerSwim;
    protected virtual void OnPlayerSwim() => PlayerSwim?.Invoke();

    // jump out water
    public event Action PlayerJumpOutWater;
    protected virtual void OnPlayerJumpOutWater() => PlayerJumpOutWater?.Invoke();
}