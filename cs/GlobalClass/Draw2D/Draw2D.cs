using Godot;
using System;
using System.Collections.Generic;

namespace GlobalClass;

/// <summary>
/// Provide a drawing workflow similar to Game Maker.
/// </summary>
[GlobalClass]
public partial class Draw2D : Node2D
{
    /// <summary>
    /// Centered the direct drawing Texture/SpriteFrames.
    /// </summary>
    [ExportCategory("Draw2D")]
    [Export]
    public bool Centered { get; set; } = true;

    /// <summary>
    /// Drawing offset.
    /// </summary>
    [Export]
    public Vector2 Offset { get; set; } = new Vector2(0f, 0f);

    /// <summary>
    /// Flip the drawing result horizontally.
    /// </summary>
    [Export]
    public bool FlipH { get; set; } = false;

    /// <summary>
    /// Flip the drawing result vertically.
    /// </summary>
    [Export]
    public bool FlipV { get; set; } = false;

    /// <summary>
    /// The max count of drawing task.
    /// Changing it in the runtime is invalid.
    /// </summary>
    [Export]
    public int MaxDrawingTask { get; set; } = 32;

    /// <summary>
    /// Process callback mode.
    /// </summary>
    public enum Draw2DProcessCallback
    {
        Idle,
        Physics,
        Manual
    }

    /// <summary>
    /// Draw2D Process callback mode.
    /// </summary>
    [Export]
    public Draw2DProcessCallback ProcessCallback { get; set; } =
        Draw2DProcessCallback.Idle;

    /// <summary>
    /// Run process only if visible.
    /// </summary>
    [Export]
    public bool VisibleOnly { get; set; } = true;

    private List<Action<Drawer>> QueuedDrawingTasks = new();
    private List<Drawer> QueuedDrawers = new();

    /// <summary>
    /// Clear all the queued drawing tasks.
    /// </summary>
    protected void ClearQueuedDraw() => QueuedDrawingTasks.Clear();

    /// <summary>
    /// Managed by Draw2D, deal with specific drawing task.
    /// </summary>
    protected partial class Drawer : Node2D
    {
        private Draw2D Root;

        public Drawer(Draw2D root) : base()
        {
            Root = root;
            Root.CallDeferred("add_child", this);
        }

        public Action DrawingTask { get; set; }

        public override void _Draw() => DrawingTask?.Invoke();
    }

    public Draw2D() : base()
    {
        TreeEntered += () =>
        {
            if (QueuedDrawers.Count > 0)
                return;

            for (int i = 0; i < MaxDrawingTask; i++)
                QueuedDrawers.Add(new Drawer(this));
        };

    }

    /// <summary>
    /// Main draw method, called automatically by default process callback mode.
    /// For manual mode you have to set up draw tasks and call this method manually.
    /// </summary>
    public void Redraw()
    {
        for (int i = 0; i < QueuedDrawers.Count; i++)
        {
            if (i >= QueuedDrawingTasks.Count)
            {
                if (!QueuedDrawers[i].Visible)
                    break;

                QueuedDrawers[i].Hide();
                continue;
            }

            QueuedDrawingTasks[i].Invoke(QueuedDrawers[i]);
            QueuedDrawers[i].Show();
            QueuedDrawers[i].QueueRedraw();
        }
    }

    /// <summary>
    /// Create drawing tasks in process.
    /// The base method will clear the tasks,
    /// reset the blend mode, material modulate, transform, etc.
    /// Override to implement.
    /// </summary>
    /// <returns>true to update draw, false to keep last draw.</returns>
    public virtual bool DrawProcess(double delta)
    {
        ClearQueuedDraw();
        ResetBlendMode();
        ResetMaterial();
        ResetModulate();
        ResetTransform();

        return true;
    }

    private void ProcessDrawing(double delta)
    {
        if (VisibleOnly && !Visible) { return; }

        if (DrawProcess(delta))
            Redraw();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (ProcessCallback == Draw2DProcessCallback.Idle)
            ProcessDrawing(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (ProcessCallback == Draw2DProcessCallback.Physics)
            ProcessDrawing(delta);
    }
}