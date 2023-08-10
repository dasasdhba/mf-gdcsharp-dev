using System.Collections.Generic;

namespace Component.BT;

/// <summary>
/// Parallel Task node in behavior tree that performs tasks parallelly.
/// When every task has fininshed, return State.Ok if any task is finally successful.
/// </summary>
public partial class ParallelNode : TaskNode
{
    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public ParallelNode(BTNode[] tasks) : base(tasks) { }

    private Dictionary<BTNode, bool> QueuedTask = new();
    private bool Ok = false;

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
                    QueuedTask.Add(task, isOk);
                    Ok = isOk || Ok;
                }
            }
        }

        if (QueuedTask.Count < Tasks.Length)
        {
            return State.Processing;
        }

        bool finalOk = Ok;

        QueuedTask.Clear();
        Ok = false;

        return finalOk ? State.Ok : State.Failed;
    }
}