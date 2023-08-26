using Spawner;

namespace Utils;

/// <summary>
/// Useful functions for spawner init default values.
/// </summary>
public static partial class USpawner
{
    /// <summary>
    /// The spawner is used for init only.
    /// </summary>
    public static void InitOnly(Spawner2D spawner)
    {
        spawner.SpawnOnce = true;
        spawner.ProcessCallback = Spawner2D.SpawnerProcessCallback.Ready;
    }

    /// <summary>
    /// The spawner will only spawn when in screen.
    /// </summary>
    public static void SpawnInScreen(Spawner2D spawner, float eps = 0f)
    {
        spawner.SpawnInScreen = true;
        spawner.InScreenEps = eps;
    }

    /// <summary>
    /// The spawner is used for init only when in screen.
    /// </summary>>
    public static void InitOnlyInScreen(Spawner2D spawner, float eps = 0f)
    {
        InitOnly(spawner);
        SpawnInScreen(spawner, eps);
    }
}