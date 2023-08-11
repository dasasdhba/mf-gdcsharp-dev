using System;

namespace Component.BT;

/// <summary>
/// Alwasy perform an action until a fixed timer over.
/// </summary>
public partial class TimerActionNode : BTNode
{
    protected Action ActionMethod;

    private double Timer = 0;
    private double Time;

    /// <summary>
    /// Construct with a time argument and an action method.
    /// </summary>
    public TimerActionNode(double time, Action action) =>
        (Time, ActionMethod) = (time, action);

    public override void Reset() => Timer = 0;

    public override State Perform(double delta)
    {
        Timer += delta;
        ActionMethod?.Invoke();

        if (Timer >= Time)
        {
            Timer = 0;
            return State.Ok;
        }

        return State.Processing;
    }
}