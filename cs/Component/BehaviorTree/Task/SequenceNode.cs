namespace Component.BT;

/// <summary>
/// Sequence Task node in behavior tree that performs task in a sequence.
/// </summary>
public partial class SequenceNode : TaskNode
{
    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public SequenceNode(BTNode[] tasks) : base(tasks) { }

    private int Current = 0;

    public override void Reset()
    {
        base.Reset();

        Current = 0;
    }

    public override State Perform(double delta)
    {
        for(int i=Current;i<Tasks.Length;i++)
        {
            State result = Tasks[i].Perform(delta);
            if (result != State.Ok)
            {
                Current = (result == State.Processing)? i : 0;
                return result;
            }
        }

        Current = 0;
        return State.Ok;
    }
}