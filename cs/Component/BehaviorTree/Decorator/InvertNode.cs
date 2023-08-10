namespace Component.BT;

/// <summary>
/// Invert child node's ok and failed.
/// </summary>
public partial class InvertNode : DecoratorNode
{
    /// <summary>
    /// Construct with a BTNode instance.
    /// </summary>
    public InvertNode(BTNode node) : base(node) { }

    public override State Perform(double delta)
    {
        State result = Child.Perform(delta);
        if (result == State.Processing)
        {
            return result;
        }

        return (result == State.Ok) ? State.Failed : State.Ok;
    }
}