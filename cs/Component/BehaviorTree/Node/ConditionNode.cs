namespace Component.BT;

/// <summary>
/// Condition node in behavior tree that check a condition.
/// construct with a bool method.
/// </summary>
public partial class ConditionNode : BTNode
{
    public delegate bool Condition();
    protected Condition ConditionMethod;

    /// <summary>
    /// construct with a bool method.
    /// </summary>
    public ConditionNode(Condition cond) => ConditionMethod = cond;

    public override State Perform(double delta)
        => ConditionMethod?.Invoke() == true ? State.Ok : State.Failed;

}