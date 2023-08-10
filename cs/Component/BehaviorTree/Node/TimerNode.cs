namespace Component.BT;

/// <summary>
/// Process a fixed timer.
/// </summary>
public partial class TimerNode : BTNode
{
    private double Timer = 0;
    private double Time;

    /// <summary>
    /// Construct with a time arguments.
    /// </summary>
    public TimerNode(double time) => Time = time;

    public override State Perform(double delta)
    {
        Timer += delta;

        if (Timer >= Time)
        {
            Timer = 0;
            return State.Ok;
        }

        return State.Processing;
    }
}