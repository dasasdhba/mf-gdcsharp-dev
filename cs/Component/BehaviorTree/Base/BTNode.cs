namespace Component.BT;

/// <summary>
/// Base node of behavior tree.
/// </summary>
public abstract partial class BTNode
{
    /// <summary>
    /// Behavior tree node's processing state
    /// </summary>
    public enum State
    {
        Processing,
        Ok,
        Failed = -1
    };

    /// <summary>
    /// Perform behavior and get result state.
    /// </summary>
    public abstract State Perform(double delta);
}
