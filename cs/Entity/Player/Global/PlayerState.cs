using System.Collections.Generic;

namespace Entity.Player;

/// <summary>
/// Manage players and global datas.
/// </summary>
public static partial class PlayerState
{
    /// <summary>
    /// Data structure contains player entity and its global data.
    /// </summary>
    public struct PlayerEntityData
    {
        /// <summary>
        /// Player Entity
        /// </summary>
        public Player Entity;

        /// <summary>
        /// Player global data;
        /// </summary>
        public PlayerData Data;

        public PlayerEntityData(Player entity)
        {
            Entity = entity;
            Data = entity.GlobalData;
        }
    }

    private static List<PlayerEntityData> PlayerList = new() { };

    /// <summary>
    /// Get all player entity datas.
    /// </summary>
    /// <returns>The list of PlayerEntityData structures.</returns>
    public static List<PlayerEntityData> GetPlayerEntityDatas() => PlayerList;

    /// <summary>
    /// Get player entity data through player name.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    /// <returns>The target PlayerEntityData structure.</returns>
    public static PlayerEntityData? GetPlayerEntityData(string playerName)
    {
        foreach (PlayerEntityData p in PlayerList)
        {
            if (p.Data.PlayerName == playerName) { return p; }
        }

        return null;
    }

    /// <summary>
    /// Get player entity through player name.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    /// <returns>The player entity.</returns>
    public static Player GetPlayerEntity(string playerName)
    {
        Player player = GetPlayerEntityData(playerName)?.Entity;
        if (!player.IsValid()) { return null; }

        return player;
    }

    /// <summary>
    /// Get player global data through player name.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    /// <returns>The player global data.</returns>
    public static PlayerData GetPlayerData(string playerName) => GetPlayerEntityData(playerName)?.Data;

    /// <summary>
    /// Whether a specific player name exists in the current state system.
    /// </summary>
    public static bool HasPlayer(string playerName) => GetPlayerEntityData(playerName) != null;

    /// <summary>
    /// Query whether a specific player entity exists through player name.
    /// </summary>
    public static bool HasPlayerEntity(string playerName) => GetPlayerEntity(playerName) != null;

    /// <summary>
    /// Add a new player entity to the state system.
    /// </summary>
    /// <param name="player">The player entity to add.</param>
    public static void AddPlayer(Player player)
    {
        for (int i=0; i<PlayerList.Count; i++)
        {
            if (PlayerList[i].Data.PlayerName == player.GlobalData.PlayerName)
            {
                PlayerList[i] = new(player);
                return;
            }
        }

        PlayerList.Add(new PlayerEntityData(player));
    }

    /// <summary>
    /// Remove a specific player through player name.
    /// </summary>
    /// <param name="playerName">The player name.</param>
    public static void RemovePlayer(string playerName)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].Data.PlayerName == playerName)
            {
                PlayerList.RemoveAt(i);
                return;
            }
        }
    }

}