using Entity;
using Entity.Player;

namespace Spawner.Player;

/// <summary>
/// Spawner spawns PlayerPlatformer.
/// </summary>
/// <typeparam name="T">PlayerPlatformer</typeparam>
public partial class PlayerPlatformerSpawner<T> : Spawner2D where T :PlayerPlatformer, new()
{

    protected override Entity2D Spawn()
    {
        return new T() { Transform = Transform };
    }
}