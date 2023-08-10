namespace Component.BT;

/// <summary>
/// Process node in behavior tree that performs a processing behavior.
/// construct with a BT.Node.State method with double delta argument.
/// </summary>
public partial class ProcessNode : BTNode
{
    public delegate State Process(double delta);
    protected Process ProcessMethod;

    /// <summary>
    /// construct with a BT.Node.State method with double delta argument.
    /// </summary>
    public ProcessNode(Process process) => ProcessMethod = process;

    public override State Perform(double delta)
        => (ProcessMethod != null) ? ProcessMethod.Invoke(delta) : State.Failed;

}