using Godot;

namespace Utils;

/// <summary>
/// Useful functions for quickly create temp timer instance for node.
/// </summary>
public static partial class UTimer
{
    /// <summary>
    /// Create a timer as node's child.
    /// </summary>
    public static Timer Create(Node node, float time, bool oneshot = true,
        bool autostart = true, bool temp = true, bool physics = false)
    {
        Timer timer = new()
        {
            WaitTime = time,
            OneShot = oneshot,
            ProcessCallback = physics ? Timer.TimerProcessCallback.Physics : Timer.TimerProcessCallback.Idle
        };

        if (autostart)
            timer.TreeEntered += () => timer.Start();

        if (temp && oneshot)
            timer.Timeout += timer.QueueFree;

        node.CallDeferred("add_child", timer);
        return timer;
    }

    /// <summary>
    /// Init ref timer with parameters if not valid.
    /// </summary>
    /// <returns>True if a new Timer has been created.</returns>
    public static bool Init(ref Timer timer, Node node, float time, 
        bool oneshot = true, bool autostart = true, bool physics = false)
    {
        if (timer != null) { return false; }
        timer = Create(node, time, oneshot, autostart, false, physics);
        return true;
    }

    /// <summary>
    /// Init or override ref timer with parameters
    /// </summary>
    /// <returns>True if a new Timer has been created.</returns>
    public static bool ForceInit(ref Timer timer, Node node, float time,
        bool oneshot = true, bool autostart = true, bool physics = false)
    {
        if (timer?.GetParent() == node)
        {
            timer.Stop();
            timer.Paused = false;
            timer.WaitTime = time;
            timer.OneShot = oneshot;
            timer.ProcessCallback = physics ? Timer.TimerProcessCallback.Physics :
                Timer.TimerProcessCallback.Idle;

            if (autostart)
                timer.Start();
            return false;
        }

        timer = Create(node, time, oneshot, autostart, false, physics);
        return true;
    }

    /// <summary>
    /// Create a physical timer as node's child.
    /// </summary>
    public static Timer CreatePhysics(Node node, float time, bool oneshot = true,
        bool autostart = true, bool temp = true) => 
        Create(node, time, oneshot, autostart, temp, true);

    /// <summary>
    /// Init ref timer with physical parameters if not valid.
    /// </summary>
    /// <returns>True if a new Timer has been created.</returns>
    public static bool InitPhysics(ref Timer timer, Node node, float time,
        bool oneshot = true, bool autostart = true) =>
        Init(ref timer, node, time, oneshot, autostart, true);

    /// <summary>
    /// Init or override ref timer with physical parameters
    /// </summary>
    /// <returns>True if a new Timer has been created.</returns>
    public static bool ForceInitPhysics(ref Timer timer, Node node, float time,
        bool oneshot = true, bool autostart = true) =>
        ForceInit(ref timer, node, time, oneshot, autostart, true);
}