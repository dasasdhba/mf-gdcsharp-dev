namespace Component.BT;

/// <summary>
/// Action node in behavior tree that performs a specific action.
/// construct with a void method.
/// </summary>
public partial class ActionNode : BTNode
{
    public delegate void Action();
    protected Action ActionMethod;

    /// <summary>
    /// construct with a void method.
    /// </summary>
    public ActionNode(Action action) => ActionMethod = action;

    public override State Perform(double delta)
    {
        ActionMethod?.Invoke();
        return State.Ok;
    }

}