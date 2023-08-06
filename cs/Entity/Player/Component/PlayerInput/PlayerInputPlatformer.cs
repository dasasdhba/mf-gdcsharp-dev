using Godot;

namespace Entity.Player;

/// <summary>
/// Handle input with a specific up direction.
/// </summary>
public partial class PlayerInputPlatformer : PlayerInput
{
    /// <summary>
    /// ActionState collections of four direction actions.
    /// </summary>
    public struct DirInputMap
    {
        public ActionState Up = new();
        public ActionState Down = new();
        public ActionState Left = new();
        public ActionState Right = new();

        public DirInputMap() { }
    }

    /// <summary>
    /// Tool method helps to compare two DirInputMap.
    /// </summary>
    protected static bool Equals(DirInputMap a, DirInputMap b)
    {
        return Equals(a.Up, b.Up) && Equals(a.Down, b.Down) && Equals(a.Left, b.Left) && Equals(a.Right, b.Right);
    }

    private DirInputMap LastInputReal = new();
    private DirInputMap LastInputResult = new();

    /// <summary>
    /// Get direction input map by an up direction.
    /// </summary>
    public DirInputMap GetDirInputMap(Vector2 UpDirection)
    {
        DirInputMap inputReal = new()
        {
            Up = new(IsPressed("Up"), IsJustPressed("Up"), IsJustReleased("Up")),
            Down = new(IsPressed("Down"), IsJustPressed("Down"), IsJustReleased("Down")),
            Left = new(IsPressed("Left"), IsJustPressed("Left"), IsJustReleased("Left")),
            Right = new(IsPressed("Right"), IsJustPressed("Right"), IsJustReleased("Right")),
        };

        // return the same if no change happened with input
        if (Equals(inputReal, LastInputReal)) { return LastInputResult; }

        LastInputReal = inputReal;

        // handle input with UpDirection
        DirInputMap result = new();
        float angle = UpDirection.Angle();
        angle = Mathf.RadToDeg(angle);

        if (angle >= -135 && angle <= -45)
        {
            result = inputReal;
        }
        else if (angle >= -45 && angle <= 45)
        {
            result.Up = inputReal.Right;
            result.Down = inputReal.Left;
            result.Left = inputReal.Up;
            result.Right = inputReal.Down;
        }
        else if (angle >= 45 && angle <= 135)
        {
            result.Up = inputReal.Down;
            result.Down = inputReal.Up;
            result.Left = inputReal.Right;
            result.Right = inputReal.Left;
        }
        else
        {
            result.Up = inputReal.Left;
            result.Down = inputReal.Right;
            result.Left = inputReal.Down;
            result.Right = inputReal.Up;
        }

        LastInputResult = result;

        return result;
    }
}