namespace Component.BT;

// TODO: implement as a node to better handle the process.
// Util functions like UTimer may be necessary too.

/// <summary>
/// Root of a behavior tree running in the <c>Node._Process(double delta)</c>
/// </summary>
public partial class BTRoot
{
    protected BTNode Root;

    /// <summary>
    /// Construct with root BTNode.
    /// </summary>
    public BTRoot(BTNode node) => Root = node;

    /// <summary>
    /// Construct with loop count and root BTNode.
    /// </summary>
    public BTRoot(int loop, BTNode node) => (Loop, Root) = (loop, node);

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
            ClearLoopCount();
        }
    }
    private int _Loop = 0;

    /// <summary>
    /// Clear current loop count.
    /// </summary>
    public void ClearLoopCount() => Count = 0;

    private int Count = 0;

    /// <summary>
    /// Reset the root BTNode.
    /// </summary>
    public void Reset()
    {
        Root.Reset();
        ClearLoopCount();
    }

    /// <summary>
    /// Is current behavior alive or not.
    /// </summary>
    public bool IsAlive() => Processing;

    private bool Processing = false;

    /// <summary>
    /// Process the behavior tree
    /// </summary>
    /// <returns>True if alive.</returns>
    public bool Process(double delta)
    {
        if (Loop > 0 && Count >= Loop) 
        {
            Processing = false;
            return false; 
        }

        BTNode.State result = Root.Perform(delta);
        if (Loop > 0 && result != BTNode.State.Processing)
        {
            Count++;
        }

        Processing = true;
        return true;
    }
}