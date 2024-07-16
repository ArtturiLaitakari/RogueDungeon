using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
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
        if (Input.GetButtonDown("Fire3")) GameController.instance.NextHero();
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
