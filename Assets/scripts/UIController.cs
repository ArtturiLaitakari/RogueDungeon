using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public UnityEngine.UI.Text killsText;
    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Text livesText;

    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) TogglePause();
    }

    public void SetKills(int p) => killsText.text = $"Kills: {p}";
    public void SetHealth(int p, int m) => healthText.text = $"Health: {p}/{m}";
    public void SetLives(int p, int m) => livesText.text = $"Lives: {p}/{m}";

    public void TogglePause() 
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
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
