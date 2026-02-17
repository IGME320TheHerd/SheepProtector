using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//public enum State
//{
//    Playing,
//    Paused,
//    Start,
//    End
//}

public class SceneManager : MonoBehaviour
{

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsPanel;

    public static SceneManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
