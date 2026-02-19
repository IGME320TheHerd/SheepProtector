/// 
/// SCENEMANAGER - written by Nao (sacanthias)
/// PURPOSE - organize game state switches based off our FSM diagram
/// LAST UPDATED - 2/18/26
/// ________________________________________

using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class SceneManager : MonoBehaviour
{
    // list of screens to reference later
    [SerializeField] private List<GameObject> screens;

    // ScreenState enum checker +
    // Property for the current screen state that may be referenced by other scripts
    private ScreenState screenState;

    public ScreenState ScreenState
    {
        get { return screenState; }
    }
    
    public static SceneManager Instance { get; private set; }

    // Singleton instance of SceneManager
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(Instance);
    }

    // Start initializes the key fields that will be used for state switching later on
    private void Start()
    {
        // screenState is set to start by default
        screenState = ScreenState.Start;

        // Fills the scenes list with each panel "screen"
        // Each panel is the direct child of the Canvas element
        screens = new List<GameObject>();

        // Resets every panel in the scenes list to "hidden".
        // This is the default state for panels.
        foreach(GameObject panel in GameObject.FindGameObjectsWithTag("Screen"))
        {
            screens.Add(panel);
            panel.SetActive(false);
            Debug.Log("Added " + panel.name);
        }

        SwitchScreen("StartScreen");
        Debug.Log("Switched to start screen");
    }

    /// <summary>
    /// Quits the game application and closes the window.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Toggles the Pause Screen on and off based off a button input.
    /// </summary>
    public void PauseToggle()
    {
        // checks the current screen state
        if(screenState != ScreenState.Paused)
        {
            // timeScale = 0 essentially pauses in-game time and prevents the player from moving
            Time.timeScale = 0;
            SwitchScreen("PauseMenu");
        }
        else
        {
            Time.timeScale = 1;
            SwitchScreen("Playing");
            screenState = ScreenState.Playing;
        }
    }

    /// <summary>
    /// Switches to a specified screen based on the string passed.
    /// </summary>
    /// <param name="sceneName">The name of the screen to switch to (case/spelling sensitive)</param>
    public void SwitchScreen(string screenName)
    {
        // nullable GameObject to avoid throwing a NullReferenceException error
        GameObject? toChange = null;

        // foreach loop goes through each GameObject in screens list and checks if they have the same name;
        // hence why the 
        // i'm aware this is kinda computationally expensive, but since it's called so sparingly, i did it anyway lol
        foreach (GameObject panel in screens)
        {
            if (panel.name == screenName)
            {
                toChange = panel;
            }

            panel.SetActive(false);
        }

        // if statement checks if toChange is null to avoid throwing a NullReferenceException error
        // setting timeScale to 0 essentially pauses in-game time and prevents the player from moving;
        // here i use it as a failsafe of sorts
        if (toChange != null)
        {
            toChange.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        // this is really messy, but it basically checks the string passed through and changes the screen state accordingly
        screenState = screenName switch
        {
            "StartScreen" => ScreenState.Playing,
            "MainMenu" => ScreenState.Main,
            "PauseMenu" => ScreenState.Paused,
            "SettingsMenu" => ScreenState.Paused,
            "EndScreen" => ScreenState.End,
            // default case is used to set everything back to the playing state
            _ => ScreenState.Playing,
        };
    }
}
