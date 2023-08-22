using Godot;
using System.Collections.Generic;

namespace GlobalClass;

/// <summary>
/// Set and play animation uniformly with child AnimatedSprite2D.
/// </summary>
[GlobalClass]
public partial class UniformAnimatedSprite2D : Node2D
{
    [ExportCategory("UniformAnimatedSprite2D")]
    [ExportGroup("Animation")]
    [Export]
    public string Animation
    {
        get => _Animation;
        set
        {
            _Animation = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.Animation = value;
        }
    }
    private string _Animation = "";

    /// <summary>
    /// This property is only useful for setting,
    /// so it will not be updated automatically.
    /// </summary>
    [Export]
    public int Frame
    {
        get => _Frame;
        set
        {
            if (value < 0) return;

            _Frame = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.Frame = value;
        }
    }
    private int _Frame = 0;

    /// <summary>
    /// This property is only useful for setting,
    /// so it will not be updated automatically.
    /// </summary>
    public float FrameProgress
    {
        get => _FrameProgress;
        set
        {
            _FrameProgress = Mathf.Clamp(value, 0f, 1f);

            foreach (AnimatedSprite2D spr in Sprites)
                spr.FrameProgress = _FrameProgress;
        }
    }

    private float _FrameProgress = 0f;

    [Export]
    public float SpeedScale
    {
        get => _SpeedScale;
        set
        {
            if (value < 0f) return;

            _SpeedScale = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.SpeedScale = value;
        }
    }
    private float _SpeedScale = 1f;

    [ExportGroup("Offset")]
    [Export]
    public bool Centered
    {
        get => _Centered;
        set
        {
            _Centered = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.Centered = value;
        }
    }
    private bool _Centered = true;

    [Export]
    public Vector2 Offset
    {
        get => _Offset;
        set
        {
            _Offset = value;
            foreach (AnimatedSprite2D spr in Sprites)
            {
                if (spr is AnimatedSpriteOffset2D sprOffset)
                    sprOffset.BaseOffset = value;
                else
                    spr.Offset = value;
            }
        }
    }
    private Vector2 _Offset = new(0f, 0f);

    [Export]
    public bool FlipH
    {
        get => _FlipH;
        set
        {
            _FlipH = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.FlipH = value;
        }
    }
    private bool _FlipH = false;

    [Export]
    public bool FlipV
    {
        get => _FlipV;
        set
        {
            _FlipV = value;
            foreach (AnimatedSprite2D spr in Sprites)
                spr.FlipV = value;
        }
    }
    private bool _FlipV = false;

    /// <summary>
    /// Managed child AnimatedSprite2D nodes.
    /// </summary>
    protected List<AnimatedSprite2D> Sprites = new();

    public UniformAnimatedSprite2D() : base()
    {
        ChildEnteredTree += AddSpriteNode;
        ChildExitingTree += RemoveSpriteNode;
    }

    protected void AddSpriteNode(Node node)
    {
        if (node is AnimatedSprite2D sprite)
        {
            Sprites.Add(sprite);
            sprite.Animation = Animation;
            sprite.Frame = Frame;
            sprite.SpeedScale = SpeedScale;
            sprite.Centered = Centered;
            sprite.FlipH = FlipH;
            sprite.FlipV = FlipV;
            if (sprite is AnimatedSpriteOffset2D sprOffset)
                sprOffset.BaseOffset = Offset;
            else
                sprite.Offset = Offset;
        }
    }

    protected void RemoveSpriteNode(Node node)
    {
        if (node is AnimatedSprite2D sprite)
            Sprites.Remove(sprite);
    }

    public void Pause()
    {
        foreach (AnimatedSprite2D sprite in Sprites)
            sprite.Pause();
    }

    public void Play(string name = null, float customSpeed = 1f, bool fromEnd = false)
    {
        if (name != null)
            _Animation = name;
        foreach (AnimatedSprite2D sprite in Sprites)
            sprite.Play(name, customSpeed, fromEnd);
    }

    public void PlayBackwards(string name = null)
    {
        if (name != null)
            _Animation = name;
        foreach (AnimatedSprite2D sprite in Sprites)
            sprite.PlayBackwards(name);
    }

    public void SetFrameAndProgress(int frame, float progress)
    {
        if (frame < 0)
            return;

        _Frame = frame;
        _FrameProgress = Mathf.Clamp(progress, 0f, 1f);
        foreach (AnimatedSprite2D sprite in Sprites)
            sprite.SetFrameAndProgress(_Frame, _FrameProgress);
    }

    public void Stop()
    {
        foreach (AnimatedSprite2D sprite in Sprites)
            sprite.Stop();
    }
}