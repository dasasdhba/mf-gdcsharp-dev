using Godot;
using Asset;
using System.Collections.Generic;

namespace Component;

// TODO: the sound will stuck a little on playing sometimes,
// maybe we need to keep the player node if necessary.

/// <summary>
/// AudioStream manager component.
/// Useful for entity to manage and play audio.
/// </summary>
public partial class AudioStreamManager
{

    /// <summary>
    /// Whether to use AudioStreamPlayer2D.
    /// </summary>
    public bool Enable2D { get; set; } = false;

    // default Bus
    protected string Bus = "Sound";

    // change bus
    public void ChangeBusToSound() => Bus = "Sound";
    public void ChangeBusToMusic() => Bus = "Music";

    // root node
    private Node Root;

    // buffered audio stream
    protected Dictionary<string, AudioStream> BufferedStreams = new();

    /// <summary>
    /// Construct with root node.
    /// The AudioStreamPlayer/AudioStreamPlayer2D will be generated depending on the root.
    /// </summary>
    public AudioStreamManager(Node root) => Root = root;

    /// <summary>
    /// Construct with root node and preload AudioStreamHolder[].
    /// The AudioStreamPlayer/AudioStreamPlayer2D will be generated depending on the root.
    /// </summary>
    public AudioStreamManager(Node root, AudioStreamHolder[] preloadStreams)
    {
        Root = root;
        foreach (AudioStreamHolder stream in preloadStreams) 
        { 
            string name = stream.GetType().Name;
            BufferedStreams.Add(name, stream.GetAudioStream());
        }
    }

    /// <summary>
    /// Preload from AudioStreamHolder.
    /// </summary>
    public void Preload(AudioStreamHolder holder)
    {
        string name = holder.GetType().Name;
        if (!BufferedStreams.ContainsKey(name))
        {
            BufferedStreams.Add(name, holder.GetAudioStream());
        }
    }

    /// <summary>
    /// Preload from AudioStreamHolder[].
    /// </summary>
    public void Preload(AudioStreamHolder[] preloadStreams)
    {
        foreach (AudioStreamHolder stream in preloadStreams)
        {
            Preload(stream);
        }
    }

    /// <summary>
    /// Clear buffered streams.
    /// </summary>
    public void ClearCache() => BufferedStreams.Clear();

    /// <summary>
    /// Node for holding AudioStreamPlayer.
    /// </summary>
    protected partial class PlayerManager : Node
    {
        /// <summary>
        /// destroy if no child player
        /// </summary>
        public bool Destroy { get; set; } = false;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (Destroy && GetChildCount() == 0) { QueueFree(); }
        }
    }

    /// <summary>
    /// Node2D for holding Player2D.
    /// </summary>
    protected partial class PlayerManager2D : Node2D
    {
        /// <summary>
        /// destroy if no child player
        /// </summary>
        public bool Destroy = false;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (Destroy && GetChildCount() == 0) { QueueFree(); }
        }
    }

    // set player manager
    private PlayerManager _PlayerManager;
    protected PlayerManager GetPlayerManager()
    {
        if (_PlayerManager != null) { return _PlayerManager; }

        _PlayerManager = new PlayerManager();
        Root.CallDeferred("add_sibling", _PlayerManager);
        return _PlayerManager;
    }

    private PlayerManager2D _PlayerManager2D;
    protected PlayerManager2D GetPlayerManager2D()
    {
        if (_PlayerManager2D != null) { return _PlayerManager2D; }

        _PlayerManager2D = new PlayerManager2D();
        Root.CallDeferred("add_child", _PlayerManager2D);
        return _PlayerManager2D;
    }

    // get new player
    protected AudioStreamPlayer GetAudioStreamPlayer(AudioStream stream, bool stopAfterFree = false)
    {
        AudioStreamPlayer player = new()
        {
            Stream = stream,
            Bus = Bus
        };

        player.TreeEntered += () => player.Play();
        player.Finished += player.QueueFree;
        player.SetMeta("StopAfterFree", stopAfterFree);

        return player;
    }

    protected AudioStreamPlayer2D GetAudioStreamPlayer2D(AudioStream stream, bool stopAfterFree = false)
    {
        AudioStreamPlayer2D player = new()
        {
            Stream = stream,
            Bus = Bus
        };

        player.TreeEntered += () => player.Play();
        player.Finished += player.QueueFree;
        player.SetMeta("StopAfterFree", stopAfterFree);

        return player;
    }

    /// <summary>
    /// Get AudioStream with an AudioStreamHolder class.
    /// A preloaded AudioStream will be loaded from buffer,
    /// or the new AudioStream will be added to the buffer.
    /// </summary>
    /// <typeparam name="T">The AudioStreamHolder to load.</typeparam>
    /// <returns>The AudioStream.</returns>
    protected AudioStream GetAudioStream<T>() where T :AudioStreamHolder, new()
    {
        // use buffer first
        AudioStreamHolder holder = new T();
        string name = holder.GetType().Name;

        AudioStream stream;
        if (BufferedStreams.ContainsKey(name))
        {
            stream = BufferedStreams[name];
        }
        else
        {
            stream = holder.GetAudioStream();
            BufferedStreams.Add(name, stream);
        }

        return stream;
    }

    /// <summary>
    /// Play AudioStream, for looped sample it is recommended to enable stopAfterFree.
    /// </summary>
    /// <typeparam name="T">The AudioStreamHolder to play.</typeparam>
    /// <param name="stopAfterFree">Whether to immediately stop after the manager is freed.</param>
    /// <returns>The generated AudioStreamPlayer</returns>
    public AudioStreamPlayer PlayStream<T>(bool stopAfterFree = false) where T : AudioStreamHolder, new()
    {
        if (_Free) { return null; }

        AudioStream stream = GetAudioStream<T>();

        AudioStreamPlayer player = GetAudioStreamPlayer(stream, stopAfterFree);
        GetPlayerManager().CallDeferred("add_child", player);

        return player;
    }

    /// <summary>
    /// Play AudioStream with Player2D, for looped sample it is recommended to enable stopAfterFree.
    /// </summary>
    /// <typeparam name="T">The AudioStreamHolder to play.</typeparam>
    /// <param name="stopAfterFree">Whether to immediately stop after the manager is freed.</param>
    /// <returns>The generated AudioStreamPlayer2D</returns>
    public AudioStreamPlayer2D PlayStream2D<T>(bool stopAfterFree = false) where T : AudioStreamHolder, new()
    {
        if (_Free) { return null; }

        AudioStream stream = GetAudioStream<T>();

        AudioStreamPlayer2D player = GetAudioStreamPlayer2D(stream, stopAfterFree);
        GetPlayerManager2D().CallDeferred("add_child", player);

        return player;
    }

    /// <summary>
    /// Play AudioStream depends on the value of <c>Enable2D</c>,
    /// for looped sample it is recommended to enable stopAfterFree.
    /// </summary>
    /// <typeparam name="T">The AudioStreamHolder to play.</typeparam>
    /// <param name="stopAfterFree">Whether to immediately stop after the manager is freed.</param>
    /// <returns>The generated AudioStreamPlayer or AudioStream2D depends on <c>Enable2D</c>.</returns>
    public Node Play<T>(bool stopAfterFree = false) where T : AudioStreamHolder, new()
    {
        return Enable2D? PlayStream2D<T>(stopAfterFree) : PlayStream<T>(stopAfterFree);
    }

    // free, should be called before Root.QueueFree()
    private bool _Free = false;

    /// <summary>
    /// Free the manager.
    /// This should be called before Root.QueueFree()
    /// if <c>Enable2D</c> is enabled.
    /// </summary>
    public void Free()
    {
        if (_Free) { return; }
        _Free = true;

        if (_PlayerManager != null)
        {
            _PlayerManager.Destroy = true;
           foreach (Node child in _PlayerManager.GetChildren())
           {
                if ((bool)child.GetMeta("StopAfterFree", false)) { child.QueueFree(); }
           }
        }

        if (_PlayerManager2D != null)
        {
            Root.RemoveChild(_PlayerManager2D);
            Root.GetParent().CallDeferred("add_child", _PlayerManager2D);
            if (Root is Node2D root2D) { _PlayerManager2D.Transform = root2D.Transform; }
 
            _PlayerManager2D.Destroy = true;
            foreach (Node child in _PlayerManager2D.GetChildren())
            {
                if ((bool)child.GetMeta("StopAfterFree", false)) { child.QueueFree(); }
            }
        }
    }
}