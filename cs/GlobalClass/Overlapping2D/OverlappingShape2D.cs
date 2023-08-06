using Godot;

namespace GlobalClass;

/// <summary>
/// OverlappingShape2D handled by OverlapObject2D.
/// </summary>
[GlobalClass, Tool]
public partial class OverlappingShape2D : Node2D
{

    [ExportCategory("OverlappingShape2D")]

    [Export]
    public Shape2D Shape { get; set; }

    [Export]
    public bool Disabled { get; set; } = false;

#if TOOLS
    [Export]
    public Color DebugColor { get; set; } = new Color("9900b36b");

    public override void _Process(double delta)
    {
        if (!Engine.IsEditorHint()) { return; }

        QueueRedraw();
    }

    public override void _Draw()
    {
        if (!Engine.IsEditorHint()) { return; }

        Color DebugDrawColor = Disabled? 
            new Color(DebugColor.R, DebugColor.R, DebugColor.R, DebugColor.A) : DebugColor;
        Shape.Draw(GetCanvasItem(), DebugDrawColor);
    }
#endif

}