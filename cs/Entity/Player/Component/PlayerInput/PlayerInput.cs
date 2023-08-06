using Godot;

namespace Entity.Player;

/// <summary>
/// Handle basic player input.
/// </summary>
public partial class PlayerInput
{
    /// <summary>
    /// The state of an action which contains three bool property:
    /// <c>Pressed</c>, <c>JustPressed</c> and <c>JustReleased</c>.
    /// </summary>
    public struct ActionState
    {
        public bool Pressed { get; set; } = false;
        public bool JustPressed { get; set; } = false;
        public bool JustReleased { get; set; } = false;

        public ActionState() { }
        public ActionState(bool a, bool b, bool c) => (Pressed, JustPressed, JustReleased) = (a, b, c);
    }

    /// <summary>
    /// Tool method helps to compare two ActionState.
    /// </summary>
    protected static bool Equals(ActionState a, ActionState b)
    {
        return a.Pressed == b.Pressed && a.JustPressed == b.JustPressed && a.JustReleased == b.JustReleased;
    }

    /// <summary>
    /// The input map,
    /// using godot collection is necessary to make this property compatible with editor.
    /// </summary>
    public Godot.Collections.Dictionary<string, string> InputDict = new()
    {
        // the key should not be modified.
        {"Up", "Up" },
        {"Down", "Down" },
        {"Left", "Left" },
        {"Right", "Right" },
        {"Jump", "Jump" },
        {"Fire", "Fire" }
    };

    public PlayerInput() { }

    // you can override to implement other feature such as auto control
    public virtual bool IsPressed(string key) => Input.IsActionPressed(InputDict[key]);

    public virtual bool IsJustPressed(string key) => Input.IsActionJustPressed(InputDict[key]);

    public virtual bool IsJustReleased(string key) => Input.IsActionJustReleased(InputDict[key]);

    /// <summary>
    /// get input map with <c>InputDict</c>
    /// </summary>
    public System.Collections.Generic.Dictionary<string, ActionState> GetInputMap()
    {
        System.Collections.Generic.Dictionary<string, ActionState> result = new();
        foreach (string key in InputDict.Keys)
        {
            result.Add(key, new ActionState(IsPressed(key), IsJustPressed(key), IsJustReleased(key)));
        }

        return result;
    }

}