using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public Collider enemyArea;
    public Collider playerArea;
    public float range;

    public GameObject SpawnPlayer()
    {
        return Spawn(player, playerArea);
    }

    public GameObject SpawnEnemy()
    {
        return Spawn(enemy, enemyArea);
    }

    private GameObject Spawn(GameObject obj, Collider area)
    {
        Vector3 minPoint = area.bounds.min;
        Vector3 maxPoint = area.bounds.max;

        float randomx = UnityEngine.Random.Range(minPoint.x, maxPoint.x);
        float randomz = UnityEngine.Random.Range(minPoint.z, maxPoint.z);

        Vector3 position = new Vector3(randomx, maxPoint.y, randomz);
        Collider[] colliders = Physics.OverlapSphere(position, range);

        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].CompareTag("Obstacle"))
            {
                //Destroy(colliders[i].gameObject);
                // tai
                Vector3 temp = colliders[i].gameObject.transform.position;
                temp.y = 3f;
                colliders[i].gameObject.transform.position = temp;
            }
        }

        return Instantiate(obj, position, new Quaternion());
    }
}
