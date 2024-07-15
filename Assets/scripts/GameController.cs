using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Master game control
/// </summary>
public class GameController : MonoBehaviour
{
    public Spawner spawner;

    public int kills = 0;
    public int scorePerEnemy = 1;
    public int lives = 3;
    public int enemyStartingAmount;
    public int maxEnemyAmount;
    public int currentLives;
    private int currentEnemyAmount;

    private GameObject player;
    public static GameController instance;
    public UIController ui;
    public Scene currentScene;
    public int selectedHero { get; set; } = 0;

    public bool Isometric { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        selectedHero = Memory.instance.SelectedHero;
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.buildIndex > 0) StartGame();
    }
    void Awake() => instance = this;
    public void StartGame()
    {
        for (int i = 0; i < enemyStartingAmount; i++)
        {
            spawner.SpawnEnemy(); // todo type
        }
        player = spawner.SpawnPlayer(selectedHero);
        var hp = player.GetComponent<Health>();
        kills = 0;
        ui.SetKills(kills, maxEnemyAmount);
        currentLives = lives;
        currentEnemyAmount = enemyStartingAmount;
        ui.SetLives(currentLives, lives);
        ui.SetHealth(hp.GetHealth(), hp.MaxHealth());
    }

    /// <summary>
    /// Handles the destruction of an enemy, triggering level-specific actions based on the current scene index.
    /// Updates kill counts on the user interface based on the maximum enemy amount.
    /// </summary>
    public void EnemyDestroyed()
    {    
        (currentScene.buildIndex == 1 ? (System.Action)Lvl1 : Lvl2)();
        if (maxEnemyAmount == 0) ui.SetKillsLeft(++kills, currentEnemyAmount);
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
    private void Lvl1()
    {
        if (currentEnemyAmount >= maxEnemyAmount)
        {
            maxEnemyAmount = 0;
        }
        if (maxEnemyAmount > 0)
        {
            NewEnemy();
        }
        if (currentEnemyAmount > 0 && maxEnemyAmount==0)
        {
            currentEnemyAmount--;
        }
        if (currentEnemyAmount < 1)
        {
            ui.LevelFinished();
        }
    }

    /// <summary>
    /// enemy handling for level 2
    /// </summary>
    private void Lvl2()
    {
        NewEnemy();
    }
    public void SetHealth(int current, int maxHealth)
    {
        if (current < 0) { current = 0; }
        ui.SetHealth(current, maxHealth);
    }
    public void ShowMana(float mana, float magicLimit, string spell)
    {
        ui.SetMana(mana, magicLimit, spell);
    }
    public void ShowMana(float mana, float magicLimit)
    {
        ui.SetMana(mana, magicLimit, "");
    }
    public void SetFatique(int f)
    {
        ui.SetFatique(f);
    }
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

    /// <summary>
    /// Updates the game state each frame. 
    /// Handles player respawn, showing respawn and end screens based on game conditions,
    /// and restarts the game when player lives are exhausted.
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
                    ui.SetLives(--currentLives, lives);
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
