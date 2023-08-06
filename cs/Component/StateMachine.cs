namespace Component;

/// <summary>
/// State machine tool runs in Node._Process(delta) or Node._PhysicsProcess(delta).
/// Using Process(T obj, double delta) to call the current state Process method.
/// <example>
/// For example:
/// <code>
/// public StateMachine&lt;Node&gt; MyStateMachine = new(MyState);
/// public static readonly State&lt;Node&gt; MyState = new(
///    (Node obj, double delta) => obj.StateProcess(delta)
///    );
/// public override void _Process(double delta) => MyStateMachine.Process(this, delta);
/// </code>
/// </example>
/// </summary>
/// <typeparam name="T">The class of the node.</typeparam>
public partial class StateMachine<T>
{
    /// <summary>
    /// Current state.
    /// Directly set is not allowed, using ChangeState instead.
    /// </summary>
    public State<T> State { get; private set; }

    private bool Started = false;
    private State<T> LastState = null;

    public StateMachine() { }
    public StateMachine(State<T> state) => State = state;
    public StateMachine(StateProcess<T> process) => State = new State<T>(process);

    /// <summary>
    /// Process current state.
    /// </summary>
    public void Process(T obj, double delta)
    {

        if (!Started)
        {
            Started = true;
            State?.Enter?.Invoke(obj, LastState);
        }

        State<T> newState = State?.Process?.Invoke(obj, delta);

        if (newState != null)
        {
            Started = false;
            LastState = State;
            State = newState;
            LastState.Exit?.Invoke(obj, State);
        }
    }

    /// <summary>
    /// Change current state.
    /// </summary>
    public void ChangeState(T obj, State<T> state)
    {
        Started = false;
        LastState = State;
        State = state;
        LastState?.Exit?.Invoke(obj, State);
    }

    /// <summary>
    /// Reset current state.
    /// </summary>
    public void ResetState(T obj) => ChangeState(obj, State);

}

/// <summary>
/// State process delegate.
/// Return a state to change to it, or return null to keep the current.
/// </summary>
public delegate State<T> StateProcess<T>(T obj, double delta);

/// <summary>
/// State change event called when a state enters/exits.
/// </summary>
/// <param name="state">The new/last state.</param>
public delegate void StateChange<T>(T obj, State<T> state);

/// <summary>
/// State tool runs in <c>StateMachine</c>.
/// using <c>public static readonly</c> is recommended.
/// <example>
/// For example:
/// <code>
/// public static readonly State&lt;Node&gt; MyState = new()
/// {
///     Process = (node obj, double delta) => obj.StateProcess(delta),
///     Enter = (node obj, State&lt;Node&gt; last) => obj.StateEnter(last),
///     Exit = (node obj, State&lt;Node&gt; new) => obj.StateExit(new)
/// };
/// </code>
/// </example>
/// </summary>
/// <typeparam name="T">The class of the node.</typeparam>
public partial class State<T>
{
    /// <summary>
    /// Called in <c>StateMachine.Process</c>
    /// </summary>
    public StateProcess<T> Process { get; set; }

    /// <summary>
    /// Called when state entered.
    /// </summary>
    public StateChange<T> Enter { get; set; }

    /// <summary>
    /// Called when state exited.
    /// </summary>
    public StateChange<T> Exit { get; set; }

    public State() { }
    public State(StateProcess<T> process) => Process = process;
}