using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string savePath;

    private GameState gameState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                //Make sure pause menu is closed
                //If coming from loading, enable controls
                break;
            case GameState.Paused:
                //Open pause menu, disable controls
                break;
            case GameState.MainMenu:
                //Load Main menu scene
                break;
            case GameState.GameOver:
                //Display game over screen, do not switch scenes
                break;
            case GameState.Loading:
                //Load gameplay scene, load data from save file, do startup
                //Display loading screen (pause, gameover or mainmenu)
                break;
        }
    }

    public void Save()
    {
        //TODO 
    }

    public void Load()
    {
        //TODO 
    }

    public enum GameState
    {
        Playing,
        MainMenu,
        Paused,
        Loading,
        GameOver
    }
}
