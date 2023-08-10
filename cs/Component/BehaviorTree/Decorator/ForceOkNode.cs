namespace Component.BT;

/// <summary>
/// Force child peforms ok.
/// </summary>
public partial class ForceOkNode : DecoratorNode
{
    /// <summary>
    /// Construct with a BTNode instance.
    /// </summary>
    public ForceOkNode(BTNode node) : base(node) { }

    public override State Perform(double delta)
    {
        State result = Child.Perform(delta);
        if (result == State.Processing)
        {
            return result;
        }

        return State.Ok;
    }
}