using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 2d UI handling
/// </summary>
public class UIController : MonoBehaviour
{
    public Text killsText;
    public Text healthText;
    public Text livesText;
    public Text manaText;
    public Text informationText;
    public Text fatiqueText;
    public TextMeshProUGUI abilitiesText;
    public GameObject pauseMenu;
    public GameObject levelMenu;
    public GameObject respawnScreen;
    public GameObject endScreen;
    public GameObject HUDScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI errorText;
    private int hero = 0;
    public Camera cam;
    private string[] abilities = new string[2];
    public TextMeshProUGUI descriptionText;

    /// <summary>
    /// write character descriptions on screen
    /// </summary>
    private void Start()
    {
        abilities[0] = @"Princess
Strength: 1
Agility: 3
Power: 2
Fireball
Heal
Point light";
        abilities[1] = @"General
Strength: 3
Agility: 2
Power: 1
Forcefield
Night vision";
        if (descriptionText != null)
        {
            descriptionText.text = abilities[0];
        }
    }
    /// <summary>
    /// Listen to pause button.
    /// </summary>
    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) TogglePause();
    }

    public void SetKillsLeft(int p, int l) => killsText.text = $"Kills: {p}, ({l})";
    public void SetKills(int k, int m) => killsText.text = $"Kills: {k}/{m}";
    public void SetHealth(int p, int m) => healthText.text = $"Health: {p}/{m}";
    public void SetLives(int p, int m) => livesText.text = $"Lives: {p}/{m}";
    public void SetMana(float restoreTime, float magicLimit, string spell) {
        if (restoreTime > magicLimit) manaText.text = $"Mana restoring: {restoreTime}, {spell}";
        else manaText.text = $"Mana restoring: {restoreTime}";
    }
    public void SetAbilities(Abilities abilities, ISpells spells)
    {
        var str = abilities.Strength;
        var dex = abilities.Agility;
        var pow = abilities.Power;
        abilitiesText.text = $"Str: {str}\nAg: {dex}\nPow:{pow}\nSpell1: {spells.GetAttackSpellName()}\nSpell2: {spells.GetDefenseSpellName()}";
    }
    public void SetFatique(int f) => fatiqueText.text = $"Fatique: {f}";

    /// <summary>
    /// Level cleared, open ui screen
    /// </summary>
    public void LevelFinished()
    {
        levelMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Load next level
    /// </summary>
    public void NextLevel()
    {
        Time.timeScale = 0f;
        levelMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        endScreen.SetActive(false);
    }

    /// <summary>
    /// Select other hero
    /// </summary>
    public void NextHero()
    {
        hero ^= 1;
        if(cam) cam.transform.rotation = Quaternion.Euler(0, hero * 90, 0);
        descriptionText.text = abilities[hero];
        Memory.instance.SelectedHero = hero;
    }

    /// <summary>
    /// Pause screen
    /// </summary>
    public void TogglePause()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (!levelMenu.activeInHierarchy ||
           !respawnScreen.activeInHierarchy ||
           !endScreen.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// After death, show respawn screen
    /// </summary>
    public void ShowRespawnScreen() { 
        if(respawnScreen) respawnScreen.SetActive(true); 
    }

    /// <summary>
    /// Hide respawn screen
    /// </summary>
    public void HideRespawnScreen()
    {
        if (respawnScreen) respawnScreen.SetActive(false);
    }

    /// <summary>
    /// Game over screen
    /// </summary>
    /// <param name="kills"></param>
    public void ShowEndScreen(int kills) {
        endScreen.SetActive(true);
        HUDScreen.SetActive(false);
        scoreText.text = $"Kills {kills}";
    }

    /// <summary>
    /// Start from the beginning
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// End whole game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
