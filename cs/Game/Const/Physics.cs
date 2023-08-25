namespace Game.Const;

/// <summary>
/// Store physics layer mask index constants.
/// </summary>
public static partial class Physics
{
    // the physics layer in this project

    public static readonly uint Obstacle = 1;
    public static readonly uint ObstaclePlayer = 1 << 1;
    public static readonly uint ObstacleEnemy = 1 << 2;
    public static readonly uint Entity = (uint)1 << 31;
    public static readonly uint EntityPlayer = 1 << 30;
    public static readonly uint EntityEnemy = 1 << 29;

    // the complex layer index of entities

    public static readonly uint PlayerMask = Obstacle | ObstaclePlayer;
    public static readonly uint PlayerLayer = Entity | EntityPlayer;

    public static readonly uint EnemyMask = Obstacle | ObstacleEnemy;
    public static readonly uint EnemyLayer = Entity | EntityEnemy;
}