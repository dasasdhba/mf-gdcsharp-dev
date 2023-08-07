using GlobalClass;

namespace Entity.Player;
public partial class PlayerPlatformerBody : PlatformerBody2D
{
    // event of player platformer movement
    //
    // subscriber is set up by entity

    public delegate void SignalHandler();

    // change shape
    public delegate void ChangeShapeHandler(int state);
    public event ChangeShapeHandler ChangeShape;

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
    public event SignalHandler PlayerJump;
    protected virtual void OnPlayerJump() => PlayerJump?.Invoke();

    // swim
    public event SignalHandler PlayerSwim;
    protected virtual void OnPlayerSwim() => PlayerSwim?.Invoke();

    // jump out water
    public event SignalHandler PlayerJumpOutWater;
    protected virtual void OnPlayerJumpOutWater() => PlayerJumpOutWater?.Invoke();
}