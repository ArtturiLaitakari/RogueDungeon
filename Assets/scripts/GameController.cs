using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Spawner spawner;

    public int kills = 0;
    public int scorePerEnemy = 1;
    public int lives = 3;
    public int enemyStartingAmount;
    public int maxEnemyAmount;
    private int currentLives;
    private int currentEnemyAmount;

    private GameObject player;
    public static GameController instance;
    public UIController ui;

    void Awake() { instance = this; }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemyStartingAmount; i++)
        {
            spawner.SpawnEnemy(); // todo type
        }
        player = spawner.SpawnPlayer();
        var hp = player.GetComponent<Health>();
        kills = 0;
        ui.SetKills(kills);
        currentLives = lives;
        currentEnemyAmount = enemyStartingAmount;
        ui.SetLives(currentLives, lives);
        ui.SetHealth(hp.GetHealth(), hp.MaxHealth());
    }
    public void EnemyDestroyed()
    {
        spawner.SpawnEnemy();
        ui.SetKills(++kills);
        if (currentEnemyAmount < maxEnemyAmount)
        {
            spawner.SpawnEnemy();
            currentEnemyAmount++;
        }
    }

    public void SetHealth(int current, int maxHealth)
    {
        if (current < 0) { current = 0; }
        ui.SetHealth(current, maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        if ( currentLives > 0)
        {
            if (Input.GetButtonDown("Restart"))
            {
                if (player == null)
                {
                    player = spawner.SpawnPlayer();
                    currentLives--;
                    ui.SetLives(currentLives, lives);
                }
            }
        }
    }
}
