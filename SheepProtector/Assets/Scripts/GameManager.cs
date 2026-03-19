using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string savePath;

    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject controlsScreen;
    [SerializeField]
    private GameObject loadScreen;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject pauseCanvas;

    [SerializeField]
    private GameObject eventSystemUI;

    [SerializeField]
    private string mainMenuScene;

    //This will be replaced with saving and loading!
    [SerializeField]
    private string gameScene;

    private GameObject currentScreen;

    public GameState State { get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(Instance);
        DontDestroyOnLoad(pauseCanvas);
        DontDestroyOnLoad(eventSystemUI);

        //The main menu scene should already be loaded, directly set the state
        State = GameState.MainMenu;
        pauseScreen.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (State == GameState.Playing)
            {
                SetState((int)GameState.Paused);
            } else
            {
                SetState((int)GameState.Playing);
            }
        }
    }

    public void SetState(int stateIdx)
    {
        switch ((GameState)stateIdx)
        {
            case GameState.Playing:
                //Make sure pause menu is closed
                //If coming from loading, enable controls
                //This will be replaced with saving and loading!
                if (State == GameState.MainMenu)
                {
                    Debug.Log("loading?");
                    LoadScene(gameScene);
                }

                Unpause();
                break;
            case GameState.Paused:
                //Open pause menu, pause time
                SwitchScreenAndPause(pauseScreen);
                break;
            case GameState.MainMenu:
                //Load Main Menu Screen
                LoadScene(mainMenuScene);
                break;
            case GameState.GameOver:
                //Display game over screen, do not switch scenes
                SwitchScreenAndPause(gameOverScreen);
                break;
            case GameState.Loading:
                //Load gameplay scene, load data from save file, do startup
                //Display loading screen (pause, gameover or mainmenu)
                //TODO
                break;
        }
        State = (GameState)stateIdx;
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // added to toggle on & off pause menu
    public void PauseToggle()
    {
        if(State == GameState.Paused)
        {
            SetState(0);
        }
        else
        {
            SetState(2);
        }
    }

    public void SwitchScreenAndPause(GameObject switchTo)
    {
        if (currentScreen != null)
        {
            currentScreen.SetActive(false);
        }

        Time.timeScale = 0.0f;
        switchTo.SetActive(true);
        currentScreen = switchTo;
    }

    void Unpause()
    {
        if (pauseScreen.activeSelf || currentScreen)
        {
            pauseScreen.SetActive(false);
            currentScreen = null;
        }

        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Save()
    {
        //TODO 
    }

    public void Load()
    {
        //TODO 
    }

    [Serializable]
    public enum GameState
    {
        Playing,
        MainMenu,
        Paused,
        Loading,
        GameOver
    }
}