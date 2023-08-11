using System;

namespace Component.BT;

/// <summary>
/// Alwasy perform an action until a reference timer over.
/// </summary>
public partial class TimerRefActionNode : BTNode
{
    protected Action ActionMethod;

    private double Timer = 0;
    private Func<double> TimeMethod;

    /// <summary>
    /// Construct with a double method returning the time and an action method.
    /// </summary>
    public TimerRefActionNode(Func<double> timeMethod, Action action) => 
        (TimeMethod, ActionMethod) = (timeMethod, action);

    public override void Reset() => Timer = 0;

    public override State Perform(double delta)
    {
        if (TimeMethod == null) 
        {
            Timer = 0;
            return State.Failed;
        }

        Timer += delta;
        ActionMethod?.Invoke();

        if (Timer >= TimeMethod.Invoke())
        {
            Timer = 0;
            return State.Ok;
        }

        return State.Processing;
    }
}