using Godot;
using Spawner;
using GlobalClass;

namespace Entity;

/// <summary>
/// Base class for entities.
/// </summary>
public abstract partial class Entity2D
{
    // 2D Entity base class

    // transform for 2d init
    private Transform2D _transform = new(0f, new Vector2(0f, 0f));

    /// <summary>
    /// <c>Transform</c> property similar to Node2D.
    /// Mostly used by `Spawner2D` to init.
    /// </summary>
    public Transform2D Transform
    {
        get { return _transform; }
        set { _transform = value; }
    }

    /// <summary>
    /// <c>Rotation</c> property similar to Node2D.
    /// Mostly used by `Spawner2D` to init.
    /// </summary>
    public float Rotation
    {
        get { return _transform.Rotation; }
        set { _transform = new Transform2D(value, Scale, Skew, Position); }
    }

    /// <summary>
    /// <c>Scale</c> property similar to Node2D.
    /// Mostly used by `Spawner2D` to init.
    /// </summary>
    public Vector2 Scale
    {
        get { return _transform.Scale; }
        set { _transform = new Transform2D(Rotation, value, Skew, Position); }
    }

    /// <summary>
    /// <c>Skew</c> property similar to Node2D.
    /// Mostly used by `Spawner2D` to init.
    /// </summary>
    public float Skew
    {
        get { return _transform.Skew; }
        set { _transform = new Transform2D(Rotation, Scale, value, Position); }
    }

    /// <summary>
    /// <c>Position</c> property similar to Node2D.
    /// Mostly used by `Spawner2D` to init.
    /// </summary>
    public Vector2 Position
    {
        get { return _transform.Origin; }
        set { _transform.Origin = value; }
    }

    // binded object

    /// <summary>
    /// Container for node components to get access.
    /// </summary>
    private EntityContainer Container { get; set; }

    /// <summary>
    /// <c>Spawner2D</c> that spawns the entity.
    /// This should be binded manually by the <c>Spawner2D</c>.
    /// </summary>
    public Spawner2D Spawner { get; set; }

    /// <summary>
    /// The root node in the SceneTree of the entity.
    /// This should be set up by <c>Bind(root, true)</c>.
    /// </summary>
    public Node Root { get; private set; }

    /// <summary>
    /// Ensure getting the right container.
    /// </summary>
    private EntityContainer GetContainer()
    {
        if (Container?.GetEntity() == this) { return Container; }
        Container = new(this);
        return Container;
    }

    /// <summary>
    /// Bind the node with the entity by <c>Metadata</c>.
    /// The node can access the binded entity by:
    /// <code>
    /// Utils.Meta.GetEntity(node)
    /// </code>
    /// </summary>
    /// <param name="node">The node to bind.</param>
    /// <param name="isRoot">Whether to set the node as <c>Root</c>.</param>
    protected void Bind(Node node, bool isRoot = false)
    {
        node.SetMeta("Entity", GetContainer());
        if (isRoot) { Root = node; }
    }

    /// <summary>
    /// Free the entity. This should be called only once
    /// after <c>Init(parent)</c> has been called.
    /// </summary>
    public void Free()
    {
        if (!Inited)
        {
            GD.PushWarning("Warning: Try to free an uninited Entity.");
            return;
        }

        if (Freed)
        {
            GD.PushWarning("Warning: Try to free a freed Entity.");
            return;
        }

        EntityFree();
        Container?.Free();

        Freed = true;
    }

    private bool Freed = false;

    /// <summary>
    /// Return <c>True</c> if the entity has been inited and not freed yet.
    /// </summary>
    public bool IsValid() => Inited && !Freed;

    /// <summary>
    /// This should be implemented to free all the dependency components.
    /// <example>
    /// For example:
    /// <code>
    /// protected override void EntityFree() => Root?.QueueFree();
    /// </code>
    /// </example>
    /// This method will be called in <c>Free()</c>.
    /// </summary>
    protected abstract void EntityFree();

    /// <summary>
    /// This should be implemented to init all the dependency components.
    /// <param>
    /// This method will be called in <c>Init(parent)</c> at first.
    /// </param>
    /// </summary>
    protected abstract void SetComponents();

    /// <summary>
    /// This should be implemented to add specific node components to the SceneTree,
    /// which should be set as the children of param <c>parent</c>.
    /// Using <c>CallDeferred</c> is recommended for safety reason.
    /// <example>
    /// For example:
    /// <code>
    /// protected override void EnterTree(Node Parent) => parent.CallDeferred("add_child", Root);
    /// </code>
    /// </example>
    /// This method will be called in <c>Init(parent)</c> after <c>SetComponents()</c>.
    /// </summary>
    protected abstract void EnterTree(Node parent);

    /// <summary>
    /// Additional init events can be implemented by override this.
    /// <param>
    /// This method will be called in <c>Init(parent)</c>
    /// after <c>EnterTree(Node Parent)</c>.
    /// </param>
    /// </summary>
    protected virtual void EntityInit() { }

    /// <summary>
    /// Init the entity and add node to the SceneTree through <c>EnterTree(parent)</c>.
    /// This method should be called only once.
    /// </summary>
    public void Init(Node parent)
    {
        if (Inited) 
        {
            GD.PushWarning("Warning: Try to init an inited Entity.");
            return; 
        }
        Inited = true;

        SetComponents();
        EnterTree(parent);

        EntityInit();
    }

    private bool Inited = false;
}
