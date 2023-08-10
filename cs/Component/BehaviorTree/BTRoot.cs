namespace Component.BT;

/// <summary>
/// Root of a behavior tree running in the <c>Node._Process(double delta)</c>
/// </summary>
public partial class BTRoot
{
    protected BTNode Node;

    /// <summary>
    /// Construct with root BTNode.
    /// </summary>
    public BTRoot(BTNode node) => Node = node;

    /// <summary>
    /// Construct with loop count and root BTNode.
    /// </summary>
    public BTRoot(int loop, BTNode node) => (Loop, Node) = (loop, node);

    /// <summary>
    /// Repeat processing count. 0 times will lead to infinite loop.
    /// Set this property will reset loop count.
    /// </summary>
    public int Loop
    {
        get => _Loop;
        set
        {
            _Loop = value;
            Count = 0;
        }
    }

    private int _Loop = 0;

    private int Count = 0;

    /// <summary>
    /// Reset the root BTNode.
    /// </summary>
    public void ResetBT(BTNode node)
    {
        Node = node;
        Count = 0;
    }

    public void Process(double delta)
    {
        if (Loop > 0 && Count >= Loop) { return; }

        BTNode.State result = Node.Perform(delta);
        if (Loop > 0 && result != BTNode.State.Processing)
        {
            Count++;
        }
    }
}