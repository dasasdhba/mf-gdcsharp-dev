namespace Component.BT;

/// <summary>
/// Process a reference timer.
/// </summary>
public partial class TimerRefNode : BTNode
{
    private double Timer = 0;
    public delegate double Time();
    private Time TimeMethod;

    /// <summary>
    /// Construct with a double method returning the time.
    /// </summary>
    public TimerRefNode(Time timeMethod) => TimeMethod = timeMethod;

    public override State Perform(double delta)
    {
        if (TimeMethod == null) 
        {
            Timer = 0;
            return State.Failed;
        }

        Timer += delta;

        if (Timer >= TimeMethod.Invoke())
        {
            Timer = 0;
            return State.Ok;
        }

        return State.Processing;
    }
}