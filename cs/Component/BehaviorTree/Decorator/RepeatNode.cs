namespace Component.BT;

/// <summary>
/// Repeat child n times until failed.
/// 0 times will lead to infinite loop.
/// </summary>
public partial class RepeatNode : DecoratorNode
{
    private int Loop = 0;

    /// <summary>
    /// Construct with a BTNode instance with infinite loop.
    /// </summary>
    public RepeatNode(BTNode node) : base(node) { }

    /// <summary>
    /// Construct with repeat count and a BTNode instance.
    /// </summary>
    public RepeatNode(int loop, BTNode node) : base(node) => Loop = loop;

    private int Count = 0;

    public override State Perform(double delta)
    {
        State result = Child.Perform(delta);
        if (result == State.Processing)
        {
            return result;
        }
        else if (result == State.Failed)
        {
            Count = 0;
            return State.Failed;
        }

        if (Loop > 0) 
        { 
            Count++;
            if (Count >= Loop)
            {
                Count = 0;
                return State.Ok;
            }
        }

        return State.Processing;
    }
}