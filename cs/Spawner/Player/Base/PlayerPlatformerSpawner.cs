using Entity;
using Entity.Player;
using Godot;

namespace Spawner.Player;

/// <summary>
/// Spawner spawns PlayerPlatformer.
/// </summary>
/// <typeparam name="T">PlayerPlatformer</typeparam>
public partial class PlayerPlatformerSpawner<T> : Spawner2D<T> where T :PlayerPlatformer, new()
{
    [ExportCategory("PlayerPlatformerSpawner")]
    [Export(PropertyHint.Enum, "Left:-1, Right:1")]
    public int Direction { get; set; } = 1;

    protected override void EntitySetComponents(Entity2D entity)
    {
        ((PlayerPlatformer)entity).RootNode.SetWalkDirection(Direction);
    }
}