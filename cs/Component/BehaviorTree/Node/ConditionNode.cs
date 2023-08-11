using System;

namespace Component.BT;

/// <summary>
/// Condition node in behavior tree that check a condition.
/// construct with a bool method.
/// </summary>
public partial class ConditionNode : BTNode
{
    protected Func<bool> ConditionMethod;

    /// <summary>
    /// construct with a bool method.
    /// </summary>
    public ConditionNode(Func<bool> cond) => ConditionMethod = cond;

    public override State Perform(double delta)
        => ConditionMethod?.Invoke() == true ? State.Ok : State.Failed;

}