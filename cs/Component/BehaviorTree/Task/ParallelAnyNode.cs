using System.Collections.Generic;

namespace Component.BT;

/// <summary>
/// Parallel Any Task node in behavior tree that performs tasks parallelly.
/// Return State.Ok immediately if any task is successful.
/// </summary>
public partial class ParallelAnyNode : TaskNode
{
    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public ParallelAnyNode(BTNode[] tasks) : base(tasks) { }

    private Dictionary<BTNode, bool> QueuedTask = new();

    public override State Perform(double delta)
    {
        foreach (BTNode task in Tasks)
        {
            if (!QueuedTask.ContainsKey(task))
            {
                State result = task.Perform(delta);
                if (result != State.Processing)
                {
                    bool isOk = result == State.Ok;
                    if (isOk)
                    {
                        QueuedTask.Clear();
                        return State.Ok;
                    }
                    QueuedTask.Add(task, isOk);
                }
            }
        }

        if (QueuedTask.Count < Tasks.Length)
        {
            return State.Processing;
        }

        QueuedTask.Clear();
        return State.Failed;
    }
}