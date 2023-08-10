using System.Collections.Generic;

namespace Component.BT;

/// <summary>
/// Parallel Fallback Task node in behavior tree that performs tasks parallelly.
/// Return State.Failed immediately if any task is failed.
/// </summary>
public partial class ParallelFallbackNode : TaskNode
{
    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public ParallelFallbackNode(BTNode[] tasks) : base(tasks) { }

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
                    if (!isOk)
                    {
                        QueuedTask.Clear();
                        return State.Failed;
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
        return State.Ok;
    }
}