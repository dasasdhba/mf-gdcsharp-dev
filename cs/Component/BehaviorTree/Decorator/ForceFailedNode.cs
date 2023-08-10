namespace Component.BT;

/// <summary>
/// Force child peforms failed.
/// </summary>
public partial class ForceFailedNode : DecoratorNode
{
    /// <summary>
    /// Construct with a BTNode instance.
    /// </summary>
    public ForceFailedNode(BTNode node) : base(node) { }

    public override State Perform(double delta)
    {
        State result = Child.Perform(delta);
        if (result == State.Processing)
        {
            return result;
        }

        return State.Failed;
    }
}