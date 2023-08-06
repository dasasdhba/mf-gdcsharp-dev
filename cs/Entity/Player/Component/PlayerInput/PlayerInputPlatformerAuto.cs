using System.Collections.Generic;

namespace Entity.Player;

/// <summary>
/// Control will be handled manually.
/// Useful to create a replay.
/// </summary>
public partial class PlayerInputPlatformerAuto : PlayerInputPlatformer
{

    private Dictionary<string, ActionState> AutoInputDict = new();

    /// <summary>
    /// Set up input by the key of the <c>InputDict</c> and <c>ActionState</c>.
    /// </summary>
    /// <param name="key">The key of <c>InputDict</c></param>
    /// <param name="state">The <c>ActionState</c></param>
    public void SetAutoInput(string key, ActionState state)
    {
        if (AutoInputDict.ContainsKey(key)) { AutoInputDict[key] = state; }
        else { AutoInputDict.Add(key, state); }
    }

    /// <summary>
    /// Set up input by the key of the <c>InputDict</c> and three concrete action states.
    /// </summary>
    /// <param name="key">The key of <c>InputDict</c></param>
    public void SetAutoInput(string key, bool isPressed, bool isJustPressed, bool isJustReleased)
    {
        ActionState state = new(isPressed, isJustPressed, isJustReleased);
        SetAutoInput(key, state);
    }

    private ActionState GetAutoInput(string key)
    {
        if (AutoInputDict.ContainsKey(key)) { return AutoInputDict[key]; }
        AutoInputDict.Add(key, new ActionState());
        return AutoInputDict[key];
    }

    public override bool IsPressed(string key) => GetAutoInput(key).Pressed;
    public override bool IsJustPressed(string key) => GetAutoInput(key).JustPressed;
    public override bool IsJustReleased(string key) => GetAutoInput(key).JustReleased;
}