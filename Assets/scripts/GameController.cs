using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Master game control
/// </summary>
public class GameController : MonoBehaviour
{
    public Spawner spawner;

    public int enemyStartingAmount;
    public int maxEnemyAmount;
    public int currentLives;
    public static GameController instance;
    public UIController ui;
    public Scene currentScene;
    public int selectedHero { get; set; } = 0;

    public bool Isometric { get; set; } = true;
    public bool outdoor = false;
    public bool endless = false;
    public AudioSource waterAudio;
    public GameObject[] gifts;
    public int numberOfGifts = 0;

    private int currentEnemyAmount;
    private int kills = 0;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        selectedHero = Memory.instance.SelectedHero;
        currentScene = SceneManager.GetActiveScene();
        if (spawner == null)
        {
            spawner = GetComponentInChildren<Spawner>();
        }

        if (currentScene.buildIndex > 0)
        {
            StartGame();
        }
    }
    void Awake() => instance = this;
    public void StartGame()
    {
        for (int i = 0; i < enemyStartingAmount; i++)
        {
            spawner.SpawnEnemy();
        }

        player = spawner.SpawnPlayer(selectedHero);
        kills = 0;
        ui.SetKills(kills, maxEnemyAmount);
        currentLives = Memory.instance.CurrentLives;
        currentEnemyAmount = enemyStartingAmount;
        ui.SetLives(currentLives, Memory.instance.StartingLives);
    }

    /// <summary>
    /// Handles the destruction of an enemy, triggering level-specific actions based on the current scene index.
    /// Updates kill counts on the user interface based on the maximum enemy amount.
    /// </summary>
    public void EnemyDestroyed()
    {    
        (endless ? (Action)Lvl : LvlEndless)();
        if (currentEnemyAmount >= maxEnemyAmount)
        {
            ui.SetKillsLeft(++kills, currentEnemyAmount);
            if(kills >= maxEnemyAmount) ui.LevelFinished();
        }
        else ui.SetKills(++kills, maxEnemyAmount);

    }

    /// <summary>
    /// Spawn a new Enemy, calculate if enemy number is increased or not
    /// </summary>
    public void NewEnemy()
    {
        spawner.SpawnEnemy();
        if (currentEnemyAmount < maxEnemyAmount)
        {
            spawner.SpawnEnemy();
            currentEnemyAmount++;
        }
    }

    /// <summary>
    /// enemy handling for level 1
    /// </summary>
    private void Lvl()
    {
        if (kills >= maxEnemyAmount) ui.LevelFinished();        
        else NewEnemy();        
    }

    /// <summary>
    /// enemy handling for level 2
    /// </summary>
    private void LvlEndless() => NewEnemy();
    public void SetHealth(int current, int maxHealth)
    {
        if (current < 0) { current = 0; }
        ui.SetHealth(current, maxHealth);
    }
    public void ShowMana(float mana) => ui.SetMana(mana);
    public void ShowMana(string problem) => ui.SetMana(problem);
    public void SetFatique(int f) => ui.SetFatique(f);

    public void SetCondition(string c) => ui.SetCondition(c);

    public void SetAbilities(Abilities a, ISpells spells)
    {
        ui.SetAbilities(a, spells);
    }

    /// <summary>
    /// Select other hero
    /// </summary>
    public void NextHero()
    {
        ui.NextHero();
    }

    public void NextLevel()
    {
        Memory.instance.CurrentLives = currentLives;
        SceneManager.LoadScene(currentScene.buildIndex +1);
    }
    public void WaterAudio()
    {
        waterAudio.Play();
    }

    public bool LevelFinished()
    {
        return kills == maxEnemyAmount;
    }

    /// <summary>
    /// Updates the game state each frame. 
    /// Handles players respawn, showing respawn and end screens based on game conditions,
    /// and restarts the game when players lives are exhausted.
    /// </summary>    
    void Update()
    {
        if (currentLives > 0)
        {
            if (player == null)
            {
                ui.ShowRespawnScreen();
                if (Input.GetButtonDown("Restart"))
                {
                    player = spawner.SpawnPlayer(selectedHero);
                    ui.SetLives(--currentLives, Memory.instance.StartingLives);
                    ui.HideRespawnScreen();
                }
            } 
        } else ui.ShowEndScreen(kills);
        if (Input.GetButtonDown("Restart") && currentLives <1)
        {
            SceneManager.LoadScene(0);
        }
    }
}
