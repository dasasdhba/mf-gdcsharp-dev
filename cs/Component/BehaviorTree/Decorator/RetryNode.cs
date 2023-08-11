namespace Component.BT;

/// <summary>
/// Retry child n times until ok.
/// 0 times will lead to infinite loop.
/// </summary>
public partial class RetryNode : DecoratorNode
{
    private int Loop = 0;

    /// <summary>
    /// Construct with a BTNode instance with infinite loop.
    /// </summary>
    public RetryNode(BTNode node) : base(node) { }

    /// <summary>
    /// Construct with repeat count and a BTNode instance.
    /// </summary>
    public RetryNode(int loop, BTNode node) : base(node) => Loop = loop;

    private int Count = 0;

    public override void Reset()
    {
        base.Reset();

        Count = 0;
    }

    public override State Perform(double delta)
    {
        State result = Child.Perform(delta);
        if (result == State.Processing)
        {
            return result;
        }
        else if (result == State.Ok)
        {
            Count = 0;
            return State.Ok;
        }

        if (Loop > 0) 
        { 
            Count++;
            if (Count >= Loop)
            {
                Count = 0;
                return State.Failed;
            }
        }

        return State.Processing;
    }
}