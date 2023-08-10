namespace Component.BT;

/// <summary>
/// Base task node in behavior tree that performs behavior in tasks.
/// </summary>
public abstract partial class TaskNode : BTNode
{
    protected BTNode[] Tasks;

    /// <summary>
    /// Construct with BTNode instance Array.
    /// </summary>
    public TaskNode(BTNode[] tasks) => Tasks = tasks;
}