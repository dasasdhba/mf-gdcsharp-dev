namespace Component.BT;

/// <summary>
/// Fallback Task node in behavior tree that performs tasks
/// until a task is successfully performed.
/// </summary>
public partial class FallbackNode : TaskNode
{
    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public FallbackNode(BTNode[] tasks) : base(tasks) { }

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
            if (result != State.Failed)
            {
                Current = (result == State.Processing)? i : 0;
                return result;
            }
        }

        Current = 0;
        return State.Failed;
    }
}