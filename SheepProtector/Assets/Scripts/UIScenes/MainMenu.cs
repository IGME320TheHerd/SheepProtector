using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.SetState(0);
    }

    public void ContinueGame()
    {
        GameManager.Instance.SetState(0);
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
