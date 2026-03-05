/// 
/// GAMEMANAGER - written by Nao (sacanthias)
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
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    // list of screens to reference later
    [SerializeField] private List<GameObject> screens;
    
    public static ScreenManager Instance { get; private set; }

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
        // Fills the scenes list with each panel "screen"
        // Each panel is the direct child of the Canvas element
        screens = new List<GameObject>();

        // Retrieves the scenes
        GetScreens();

        // setting game time to zero, just in case
        Time.timeScale = 0;

        SwitchScreen("MainMenu");
    }

    /// <summary>
    /// Quits the game application and closes the window.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("Greybox Map").buildIndex);
    }

    /// <summary>
    /// Helper method for quickly retrieving all sub-panels in an individual scene.
    /// </summary>
    private void GetScreens()
    {
        screens.Clear();

        // Resets every panel in the scenes list to "hidden".
        // This is the default state for panels.
        foreach (GameObject panel in GameObject.FindGameObjectsWithTag("Screen"))
        {
            screens.Add(panel);
            panel.SetActive(false);
            Debug.Log("Added " + panel.name);
        }
    }

    /// <summary>
    /// Switches to the specified scene based on the string passed
    /// </summary>
    /// <param name="sceneName">The name of the screen to switch to (case/spelling sensitive)</param>
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

    }
}
