namespace Entity.Player;

/// <summary>
/// Player data structure.
/// Including player name and other player global data.
/// </summary>
public partial class PlayerData
{
    public string PlayerName { get; set; } = "";

    public enum State
    {
        Small,
        Super,
        Fire,
        Beet,
        Lui
    }

    public State PlayerState { get; set; } = State.Small;

    public PlayerData() { }

    public PlayerData(string playerName) => PlayerName = playerName;

}