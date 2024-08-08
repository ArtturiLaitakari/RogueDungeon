using System;
using UnityEngine;

/// <summary>
/// stores selected Hero over scenes
/// </summary>
public class Memory : MonoBehaviour
{
    public int SelectedHero { get; set; } = 0;
    public static Memory instance;
    public int StartingLives { get; set; } = 3;
    public int CurrentLives { get; set; } = 3;
    public int TotalKills { get; set; } = 0;


    void Awake()
    {
        // Varmista, että GameController on singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Säilytä tämä GameObject scenen vaihdon yli
        }
        else
        {
            Destroy(gameObject); // Jos on jo olemassa toinen instanssi, tuhoa tämä
        }
    }
}
