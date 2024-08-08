using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        Memory.instance.CurrentLives = Memory.instance.StartingLives;
        SceneManager.LoadScene("Lvl1");
    }

    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            StartGame();
        }
        if (Input.GetButtonDown("Cancel")) // select button on gamepad
        {
            QuitGame();
        }
        if (Input.GetButtonDown("Fire4")) GameController.instance.NextHero();
        if (Input.GetButtonDown("Horizontal")) GameController.instance.NextHero();
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
