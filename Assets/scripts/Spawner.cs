using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] player;
    public GameObject[] enemies;

    public Collider enemyArea;
    public Collider playerArea;
    public float range;

    void Start()
    {
        if (!enabled)
        {
            Debug.Log("This script is currently disabled.");
        }
        if (player == null || player.Length == 0)
        {
            throw new Exception("Player array is null or empty in Spawner.");
        }

        if (enemies == null || enemies.Length == 0)
        {
            throw new Exception("Enemies array is null or empty in Spawner.");
        }
    }

    /// <summary>
    /// Spawns a player GameObject at the specified index in the player array.
    /// </summary>
    /// <param name="selected">The index of the player to spawn.</param>
    /// <returns>Returns the spawned player GameObject.</returns>
    public GameObject SpawnPlayer(int selected)
    {
        return Spawn(player[selected], playerArea);
    }

    /// <summary>
    /// Spawns an enemy GameObject using the GetEnemy method and places it in the enemyArea.
    /// </summary>
    /// <returns>Returns the spawned enemy GameObject.</returns>
    public GameObject SpawnEnemy()
    {
        var enemy = GetEnemy();
        return Spawn(enemy, enemyArea);
    }

    /// <summary>
    /// Retrieves a random enemy GameObject from the enemies array.
    /// </summary>
    /// <returns>Returns a randomly selected enemy GameObject.</returns>
    private GameObject GetEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemies.Length);
        return enemies[randomIndex];
    }

    /// <summary>
    /// Spawns a GameObject within the specified area, adjusting for obstacles if necessary.
    /// </summary>
    /// <param name="obj">The GameObject to spawn.</param>
    /// <param name="area">The Collider representing the area where the GameObject can spawn.</param>
    /// <returns>Returns the spawned GameObject.</returns>
    private GameObject Spawn(GameObject obj, Collider area)
    {
        Vector3 minPoint = area.bounds.min;
        Vector3 maxPoint = area.bounds.max;

        float randomx = UnityEngine.Random.Range(minPoint.x+1, maxPoint.x-1);
        float randomz = UnityEngine.Random.Range(minPoint.z+1, maxPoint.z-1);

        Vector3 position = new Vector3(randomx, maxPoint.y, randomz);
        Collider[] colliders = Physics.OverlapSphere(position, range);

        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].CompareTag("Obstacle"))
            {
                Vector3 temp = colliders[i].gameObject.transform.position;
                temp.y = 3f;
                colliders[i].gameObject.transform.position = temp;
            }
        }
        return Instantiate(obj, position, new Quaternion());
    }
}
