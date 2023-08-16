using Godot;
using System;
using System.Collections.Generic;

namespace GlobalClass;

public partial class Draw2D : Node2D
{
    // drawing API used in DrawProcess call.

    protected enum Draw2DBlendMode
    {
        Mix,
        Add,
        Sub,
        Mul,
        PremultAlpha
    }

    private Draw2DBlendMode BlendMode = Draw2DBlendMode.Mix;

    /// <summary>
    /// Attention: Blend mode will be overrided by custom material.
    /// </summary>
    protected void SetBlendMode(Draw2DBlendMode blendMode) => BlendMode = blendMode;
    protected void ResetBlendMode() => BlendMode = Draw2DBlendMode.Mix;

    private Dictionary<Draw2DBlendMode, Material> BlendMaterialMap = new()
    {
        { Draw2DBlendMode.Mix, new CanvasItemMaterial() {
            BlendMode = CanvasItemMaterial.BlendModeEnum.Mix }
        },
        { Draw2DBlendMode.Add, new CanvasItemMaterial() { 
            BlendMode = CanvasItemMaterial.BlendModeEnum.Add } 
        },
        { Draw2DBlendMode.Sub, new CanvasItemMaterial() {
            BlendMode = CanvasItemMaterial.BlendModeEnum.Sub } 
        },
        { Draw2DBlendMode.Mul, new CanvasItemMaterial() {
            BlendMode = CanvasItemMaterial.BlendModeEnum.Mul } 
        },
        { Draw2DBlendMode.PremultAlpha, new CanvasItemMaterial() {
            BlendMode = CanvasItemMaterial.BlendModeEnum.PremultAlpha } 
        }
    };

    private Material DrawMaterial = null;

    /// <summary>
    /// Attention: Non-null material will override blend mode setting.
    /// </summary>
    protected void SetMaterial(Material material) => DrawMaterial = material;
    protected void ResetMaterial() => DrawMaterial = null;

    private Color DrawModulate = new(1f, 1f, 1f);

    protected void SetModulate(Color modulate) => DrawModulate = modulate;
    protected void SetModulateRGB(Color modulate)
    {
        DrawModulate.R = modulate.R;
        DrawModulate.G = modulate.G;
        DrawModulate.B = modulate.B;
    }
    protected void SetModulateAlpha(float alpha) => DrawModulate.A = alpha;
    protected void ResetModulate() => DrawModulate = new(1f, 1f, 1f);

    private Transform2D DrawTransform = new(0f, new Vector2(0f, 0f));

    protected void SetTransform(Transform2D transform) => DrawTransform = transform;
    protected void ResetTransform() => DrawTransform = new(0f, new Vector2(0f, 0f));
    protected void SetPosition(Vector2 pos) => DrawTransform.Origin = pos;
    protected void SetRotation(float rotation) => DrawTransform = new(rotation, 
        DrawTransform.Scale, DrawTransform.Skew, DrawTransform.Origin);
    protected void SetScale(Vector2 scale) => DrawTransform = new(DrawTransform.Rotation,
        scale, DrawTransform.Skew, DrawTransform.Origin);
    protected void SetSkew(float skew) => DrawTransform = new(DrawTransform.Rotation,
        DrawTransform.Scale, skew, DrawTransform.Origin);

    protected void AddDrawingTask(Action<Drawer> task)
    {
        Material QueuedMaterial = DrawMaterial ?? BlendMaterialMap[BlendMode];
        Color QueuedModulate = DrawModulate;
        Transform2D QueuedTransform = DrawTransform;

        QueuedDrawingTasks.Add((Drawer drawer) =>
        {
            drawer.Material = QueuedMaterial;
            drawer.Modulate = QueuedModulate;
            drawer.Transform = QueuedTransform;
            drawer.Position += Offset;
            if (FlipH)
            {
                drawer.Scale = drawer.Scale with { X = drawer.Scale.X * -1f };
                drawer.Position = drawer.Position with { X = drawer.Position.X * -1f };
            }
            if (FlipV)
            {
                drawer.Scale = drawer.Scale with { Y = drawer.Scale.Y * -1f };
                drawer.Position = drawer.Position with { Y = drawer.Position.Y * -1f };
            }
            drawer.ForceUpdateTransform();
            drawer.DrawingTask = () => task(drawer);
        });
    }

    // line

    protected void QueuedDrawLine(Vector2 from, Vector2 to, Color color, 
        float width = -1f, bool antialiased = false)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawLine(from, to, color, width, antialiased);
        });
    }

    protected void QueuedDrawMultiline(Vector2[] points, Color color, float width = -1f)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawMultiline(points, color, width);
        });
    }

    protected void QueuedDrawMultilineColors(Vector2[] points, Color[] colors, float width = -1f)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawMultilineColors(points, colors, width);
        });
    }

    protected void QueuedDrawPolyline(Vector2[] points, Color color,
        float width = -1f, bool antialiased = false)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawPolyline(points, color, width, antialiased);
        });
    }

    protected void QueuedDrawPolylineColors(Vector2[] points, Color[] colors,
        float width = -1f, bool antialiased = false)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawPolylineColors(points, colors, width, antialiased);
        });
    }

    protected void QueuedDrawDashedLine(Vector2 from, Vector2 to, Color color,
        float width = -1f, float dash = 2f, bool aligned = true)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawDashedLine(from, to, color, width, dash, aligned);
        });
    }

    // shape

    protected void QueuedDrawArc(Vector2 center, float radius, float startAngle, float endAngle,
        Color color, int pointCount = 128, float width = -1f, bool antialiased = false)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawArc(center, radius, startAngle, endAngle, pointCount,
                color, width, antialiased);
        });
    }

    protected void QueuedDrawRing(Vector2 center, float radius, Color color,
        int pointCount = 128, float width = -1f, bool antialiased = false)
    {
        QueuedDrawArc(center, radius, 0f, Mathf.Tau, color,
            pointCount, width, antialiased);
    }

    protected void QueuedDrawCircle(Vector2 center, float radius, Color color)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawCircle(center, radius, color);
        });
    }

    protected void QueuedDrawRect(Rect2 rect, Color color, bool filled = true,
        float width = -1f)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawRect(rect, color, filled, width);
        });
    }

    protected void QueuedDrawPolygon(Vector2[] points, Color[] colors,
        Vector2[] uvs = null, Texture2D texture = null)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawPolygon(points, colors, uvs, texture);
        });
    }

    protected void QueuedDrawColoredPolygon(Vector2[] points, Color color,
        Vector2[] uvs = null, Texture2D texture = null)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawColoredPolygon(points, color, uvs, texture);
        });
    }

    protected void QueuedDrawPrimitive(Vector2[] points, Color[] colors,
        Vector2[] uvs = null, Texture2D texture = null)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawPrimitive(points, colors, uvs, texture);
        });
    }

    // string

    protected void QueuedDrawString(Font font, Vector2 pos, string text,
        float width = -1f, int fontSize = 16, Color? modulate = null,
        HorizontalAlignment alignment = HorizontalAlignment.Left,
        TextServer.JustificationFlag justificationFlags =
        TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound,
        TextServer.Direction direction = TextServer.Direction.Auto,
        TextServer.Orientation orientation = TextServer.Orientation.Horizontal)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawString(font, pos, text, alignment, width, fontSize, modulate,
                justificationFlags, direction, orientation);
        });
    }

    protected void QueuedDrawStringOutline(Font font, Vector2 pos, string text,
        float width = -1f, int fontSize = 16, int size = 1, Color? modulate = null,
        HorizontalAlignment alignment = HorizontalAlignment.Left,
        TextServer.JustificationFlag justificationFlags =
        TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound,
        TextServer.Direction direction = TextServer.Direction.Auto,
        TextServer.Orientation orientation = TextServer.Orientation.Horizontal)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawStringOutline(font, pos, text, alignment, width, fontSize, size,
                modulate, justificationFlags, direction, orientation);
        });
    }

    protected void QueuedDrawMultilineString(Font font, Vector2 pos, string text,
        float width = -1f, int fontSize = 16, int maxLines = -1, Color? modulate = null,
        HorizontalAlignment alignment = HorizontalAlignment.Left,
        TextServer.LineBreakFlag brkFlags = 
        TextServer.LineBreakFlag.Mandatory | TextServer.LineBreakFlag.WordBound,
        TextServer.JustificationFlag justificationFlags =
        TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound,
        TextServer.Direction direction = TextServer.Direction.Auto,
        TextServer.Orientation orientation = TextServer.Orientation.Horizontal)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawMultilineString(font, pos, text, alignment, width, fontSize, 
                maxLines, modulate, brkFlags, justificationFlags, 
                direction, orientation);
        });
    }

    protected void QueuedDrawMultilineStringOutline(Font font, Vector2 pos, string text,
        float width = -1f, int fontSize = 16, int size = 1, int maxLines = -1, Color? modulate = null,
        HorizontalAlignment alignment = HorizontalAlignment.Left,
        TextServer.LineBreakFlag brkFlags =
        TextServer.LineBreakFlag.Mandatory | TextServer.LineBreakFlag.WordBound,
        TextServer.JustificationFlag justificationFlags =
        TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound,
        TextServer.Direction direction = TextServer.Direction.Auto,
        TextServer.Orientation orientation = TextServer.Orientation.Horizontal)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawMultilineStringOutline(font, pos, text, alignment, width, fontSize,
                maxLines, size, modulate, brkFlags, justificationFlags,
                direction, orientation);
        });
    }

    // texture
    
    protected void QueuedDrawTexture(Texture2D texture, Vector2 pos, Color? modulate = null)
    {
        if (Centered)
        {
            Vector2 texSize = new(texture.GetWidth(), texture.GetHeight());
            pos -= texSize / 2f;
        }
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTexture(texture, pos, modulate);
        });
    }

    protected void QueuedDrawTextureRect(Texture2D texture, Rect2 rect, bool tile,
        Color? modulate = null, bool transpose = false)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTextureRect(texture, rect, tile, modulate, transpose);
        });
    }

    protected void QueuedDrawTextureRectRegion(Texture2D texture, Rect2 rect, Rect2 srcRect,
        Color? modulate = null, bool transpose = false, bool clipUV = true)
    {
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTextureRectRegion(texture, rect, srcRect, modulate, transpose, clipUV);
        });
    }

    // sprite frames

    protected void QueuedDrawSpriteFrames(SpriteFrames spr, string animation, int frame, 
        Vector2 pos, Color? modulate = null)
    {
        Texture2D texture = spr.GetFrameTexture(animation, frame);
        if (Centered)
        {
            Vector2 texSize = new(texture.GetWidth(), texture.GetHeight());
            pos -= texSize / 2f;
        }
        if (spr is SpriteFramesOffset sprOffset)
        {
            if (sprOffset.Offsets.ContainsKey(animation))
            {
                foreach (SpriteFramesOffsetParam param in sprOffset.Offsets[animation]) 
                { 
                    if (param.Frame == frame)
                    {
                        pos += param.Offset;
                        break;
                    }
                }
            }
        }
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTexture(texture, pos, modulate);
        });
    }

    protected void QueuedDrawSpriteFramesRect(SpriteFrames spr, string animation, int frame,
        Rect2 rect, bool tile, Color? modulate = null, bool transpose = false)
    {
        Texture2D texture = spr.GetFrameTexture(animation, frame);
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTextureRect(texture, rect, tile, modulate, transpose);
        });
    }

    protected void QueuedDrawSpriteFramesRectRegion(SpriteFrames spr, string animation, int frame,
        Rect2 rect, Rect2 srcRect, Color? modulate = null, 
        bool transpose = false, bool clipUV = true)
    {
        Texture2D texture = spr.GetFrameTexture(animation, frame);
        AddDrawingTask((Drawer drawer) =>
        {
            drawer.DrawTextureRectRegion(texture, rect, srcRect, modulate, transpose, clipUV);
        });
    }

    // sprite2d

    protected void QueuedDrawSprite(Sprite2D spr, Vector2 pos, Color? modulate = null)
    {
        pos += spr.Offset;
        QueuedDrawTexture(spr.Texture, pos, modulate);
    }

    protected void QueuedDrawSpriteRect(Sprite2D spr, Rect2 rect, bool tile,
        Color? modulate = null, bool transpose = false)
    {
        QueuedDrawTextureRect(spr.Texture, rect, tile, modulate);
    }

    protected void QueuedDrawSpriteRectRegion(Sprite2D spr, Rect2 rect, Rect2 srcRect,
        Color? modulate = null, bool transpose = false, bool clipUV = true)
    {
        QueuedDrawTextureRectRegion(spr.Texture, rect, srcRect, modulate, transpose, clipUV);
    }

    // animated sprite

    protected void QueuedDrawAnimatedSprite(AnimatedSprite2D spr, Vector2 pos, Color? modulate = null)
    {
        pos += (spr is AnimatedSpriteOffset sprOffset)? sprOffset.BaseOffset : spr.Offset;
        QueuedDrawSpriteFrames(spr.SpriteFrames, spr.Animation, spr.Frame, pos, modulate);
    }

    protected void QueuedDrawAnimatedSpriteRect(AnimatedSprite2D spr, Rect2 rect, bool tile,
        Color? modulate = null, bool transpose = false)
    { 
        QueuedDrawSpriteFramesRect(spr.SpriteFrames, spr.Animation, spr.Frame, 
            rect, tile, modulate, transpose);
    }

    protected void QueuedDrawAnimatedSpriteRectRegion(AnimatedSprite2D spr, Rect2 rect, Rect2 srcRect,
        Color? modulate = null, bool transpose = false, bool clipUV = true)
    {
        QueuedDrawSpriteFramesRectRegion(spr.SpriteFrames, spr.Animation, spr.Frame,
            rect, srcRect, modulate, transpose, clipUV);
    }
}