using System;
using Godot;
using Entity;
using Utils;

namespace Spawner;

/// <summary>
/// Spawner2D&lt;T&gt; spawns Entity2D T.
/// </summary>
/// <typeparam name="T">Entity2D</typeparam>
public partial class Spawner2D<T> : Spawner2D where T : Entity2D, new()
{
    protected override Entity2D Spawn() => new T() { Transform = Transform };
}

/// <summary>
/// Spawner2D spawns Entity2D.
/// </summary>
public abstract partial class Spawner2D : Node2D
{

    [ExportCategory("Spawner2D")]

    [Export]
    public bool SpawnOnce { get; set; } = false;

    [Export]
    public bool SpawnInScreen { get; set; } = false;

    [Export]
    public float InScreenEps { get; set; } = 0f;

    [Export]
    public bool ManualOnly { get; set; } = false;

    /// <summary>
    /// Process callback mode.
    /// </summary>
    public enum SpawnerProcessCallback
    {
        Idle,
        Physics,
        Ready
    }

    /// <summary>
    /// Spawner Process callback mode.
    /// </summary>
    [Export]
    public SpawnerProcessCallback ProcessCallback { get; set; } =
        SpawnerProcessCallback.Idle;

    /// <summary>
    /// Emitted when entity emits <c>ComponentsReady</c>.
    /// </summary>
    public Action<Entity2D> EntityComponentsReady;
    protected virtual void OnEntityComponentsReady(Entity2D entity) => 
        EntityComponentsReady?.Invoke(entity);

    /// <summary>
    /// Emitted when entity emits <c>TreeEntered</c>.
    /// </summary>
    public Action<Entity2D> EntityTreeEntered;
    protected virtual void OnEntityTreeEntered(Entity2D entity) =>
        EntityTreeEntered?.Invoke(entity);

    /// <summary>
    /// Emitted when spawns the entity.
    /// </summary>
    public Action<Entity2D> Spawned;
    protected virtual void OnSpawned(Entity2D entity) => Spawned?.Invoke(entity);

    /// <summary>
    /// Spawn function need to override to implement.
    /// </summary>
    protected abstract Entity2D Spawn();

    /// <summary>
    /// Init the spawned entity after <c>Entity2D.SetComponents()</c> has been called.
    /// </summary>
    protected virtual void EntitySetComponents(Entity2D entity) { }

    /// <summary>
    /// Init the spawned entity after <c>Entity2D.EnterTree(parent)</c> has been called.
    /// </summary>
    protected virtual void EntityEnterTree(Entity2D entity) { }

    /// <summary>
    /// Init the spawned entity after <c>Entity2D.Init()</c> has been called.
    /// </summary>
    protected virtual void EntityInit(Entity2D entity) { }

    /// <summary>
    /// Bind spawner to the entity.
    /// </summary>
    protected void BindEntity(Entity2D entity) { entity.Spawner = this; }

    /// <summary>
    /// Spawn and init immediately.
    /// </summary>
    public Entity2D Respawn()
    {
        Entity2D result = Spawn();
        BindEntity(result);
        result.ComponentsReady += () =>
        {
            EntitySetComponents(result);
            OnEntityComponentsReady(result);
        };
        result.TreeEntered += () =>
        {
            EntityEnterTree(result);
            OnEntityTreeEntered(result);
        };
        result.Init(GetParent());
        EntityInit(result);
        OnSpawned(result);
        return result;
    }

    private bool _Spawned = false;

    /// <summary>
    /// Reset spawner.
    /// </summary>
    public void Reset() { _Spawned = false; }

    private void SpawnProcess()
    {
        if (ManualOnly || _Spawned) { return; }

        if (SpawnInScreen && !View.IsInView(this, InScreenEps)) { return; }

        Respawn();
        _Spawned = true;
        if (SpawnOnce) { QueueFree(); }
    }

    public override void _Ready()
    {
        base._Ready();
        
        SpawnProcess();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (ProcessCallback == SpawnerProcessCallback.Physics)
            SpawnProcess();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (ProcessCallback == SpawnerProcessCallback.Idle)
            SpawnProcess();
    }
}
