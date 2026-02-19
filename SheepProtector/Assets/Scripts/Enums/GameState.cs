/// 
/// GAMESTATE - written by Nao (sacanthias)
/// PURPOSE - public enumeration for tracking game state changes as outlined in our FSM diagram
/// LAST UPDATED - 2/18/26
/// ________________________________________

/// <summary>
/// Details the different game states used in this program.
/// </summary>
public enum GameState
{
    Playing,
    MainMenu,
    Paused,
    Loading,
    GameOver
}