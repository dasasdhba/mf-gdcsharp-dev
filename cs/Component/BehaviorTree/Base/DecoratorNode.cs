namespace Component.BT;

/// <summary>
/// Base decorator node in behavior tree that alter the behavior of a node.
/// </summary>
public abstract partial class DecoratorNode : BTNode
{
    protected BTNode Child;

    /// <summary>
    /// Construct with a BTNode instance.
    /// </summary>
    public DecoratorNode(BTNode node) => Child = node;

    public override void Reset() => Child.Reset();
}