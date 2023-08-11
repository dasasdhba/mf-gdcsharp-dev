using System;
using Godot;

namespace Component.BT;

// It's not worth to make it GlobalClass,
// since we can't set up the behavior tree in the editor currently.
// Maybe we can implement a better tool later.

/// <summary>
/// Behavior tree processing Node.
/// </summary>
public partial class BTRoot : Node
{
    /// <summary>
    /// Currently it's not recommended to change the root in the runtime.
    /// There is no public method can do it either,
    /// please consider using another BTRoot.
    /// </summary>
    protected BTNode Root;

    /// <summary>
    /// Construct with root BTNode.
    /// </summary>
    public BTRoot(BTNode node) => Root = node;

    /// <summary>
    /// Process callback mode.
    /// </summary>
    public enum BTRootProcessCallback
    {
        Idle,
        Physics
    }

    /// <summary>
    /// BTRoot Process callback mode.
    /// </summary>
    [ExportCategory("BTRoot")]
    [Export]
    public BTRootProcessCallback ProcessCallback { get; set; } =
        BTRootProcessCallback.Idle;

    /// <summary>
    /// Repeat processing count. 0 times will lead to infinite loop.
    /// Set this property will reset loop count.
    /// </summary>
    [Export]
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
    /// Autostart the process when entered tree.
    /// </summary>
    [Export]
    public bool Autostart { get; set; } = false;

    /// <summary>
    /// If <c>true</c>, the BTRoot is paused and will not process until
    /// it is unpaused again, even if `<c>Start()</c> is called.
    /// </summary>
    public bool Paused { get; set; } = false;

    /// <summary>
    /// Emitted when BTNode finishd processing every time.
    /// </summary>
    public event Action BTFinished;
    protected virtual void OnBTFinished() => BTFinished?.Invoke();

    /// <summary>
    /// Emitted when finished all loop processing.
    /// </summary>
    public event Action Finished;
    protected virtual void OnFinished() => Finished?.Invoke();

    /// <summary>
    /// Clear current loop count. This will not start the process.
    /// </summary>
    public void ClearLoopCount() => Count = 0;

    protected int Count = 0;

    /// <summary>
    /// Is current processing alive or not.
    /// </summary>
    public bool IsAlive() => Processing;

    protected bool Processing = false;

    /// <summary>
    /// Start processing, everything will be reset if still in processing.
    /// </summary>
    public void Start()
    {
        if (!Processing)
        {
            Processing = true;
            ClearLoopCount();
            return;
        }

        Reset();
    }

    /// <summary>
    /// Start processing, everything will be reset if still in processing.
    /// </summary>
    public void Stop()
    {
        if (Processing)
        {
            Processing = false;
            Reset();
        }
    }

    /// <summary>
    /// Reset the root BTNode and loop count.
    /// </summary>
    protected void Reset()
    {
        Root.Reset();
        ClearLoopCount();
    }

    /// <summary>
    /// Process the behavior tree.
    /// </summary>
    /// <returns>True if in processing.</returns>
    protected bool ProcessBT(double delta)
    {
        BTNode.State result = Root.Perform(delta);
        if (result != BTNode.State.Processing)
        {
            OnBTFinished();

            if (Loop > 0)
            {
                Count++;
                if (Count >= Loop)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Method running in <c>Node._Process</c> or <c>Node._PhysicsProcess</c>.
    /// </summary>
    protected void Process(double delta)
    {
        if (Paused)
            return;

        if (Processing)
        {
            Processing = ProcessBT(delta);
            if (!Processing) { OnFinished(); }
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        if (Autostart)
            Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (ProcessCallback == BTRootProcessCallback.Idle)
            Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._Process(delta);

        if (ProcessCallback == BTRootProcessCallback.Physics)
            Process(delta);
    }
}