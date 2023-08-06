using Godot;
using Entity;
using Utils;

namespace Spawner;

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

    private bool Spawned = false;

    /// <summary>
    /// Spawn function need to override to implement.
    /// </summary>
    public abstract Entity2D Spawn();

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
        result.Init(GetParent());
        return result;
    }

    /// <summary>
    /// Reset spawner.
    /// </summary>
    public void ResetSpawner() { Spawned = false; }

    private void SpawnProcess()
    {
        if (ManualOnly || Spawned) { return; }

        if (SpawnInScreen && !View.IsInView(this, InScreenEps)) { return; }

        Respawn();
        Spawned = true;
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

        SpawnProcess();
    }
}