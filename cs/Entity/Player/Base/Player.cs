namespace Entity.Player;

/// <summary>
/// Base player class.
/// </summary>
public abstract partial class Player : Entity2D
{
    /// <summary>
    /// Player global datas.
    /// </summary>
    public PlayerData GlobalData { get; set; } = new PlayerData();

    /// <summary>
    /// Access to global player state.
    /// </summary>
    protected void AccessGlobalData()
    {
        string playerName = GlobalData.PlayerName;

        if (PlayerState.HasPlayer(playerName))
        {
            GlobalData = PlayerState.GetPlayerData(playerName);
            return;
        }

        PlayerState.AddPlayer(this);
    }

}