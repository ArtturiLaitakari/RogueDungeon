using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make ruins to the game area
/// </summary>

public class SpawnRocks : MonoBehaviour
{
    public int RockCount;
    public List<GameObject> RockList = new List<GameObject>();
    public Collider Ground;
    public int y;
    public float minSize;
    public float maxSize;

    //save memory, save resourses, use the same memory space.
    private float randomX;
    private float randomZ;
    private float randomSize;
    private GameObject spawnedBlock;
    private Rigidbody rb;

    void Start()
    {
        Vector3 minPoint = Ground.bounds.min;
        Vector3 maxPoint = Ground.bounds.max;

        for (int i = 0; i < RockCount; i++)
        {
            Vector3 position = GenerateRandomPosition(minPoint, maxPoint);
            InstantiateRandomRock(position);
        }
    }

    Vector3 GenerateRandomPosition(Vector3 min, Vector3 max)
    {
        randomX = Randomize(min.x+1, max.x-1);
        randomZ = Randomize(min.z+1, max.z-1);
        return new Vector3(randomX, y, randomZ);
    }

    void InstantiateRandomRock(Vector3 position)
    {
        var index = Randomize(0, RockList.Count);
        var rock = RockList[(int)index];
        randomSize = Randomize(minSize, maxSize);
        position.y = y;
        spawnedBlock = Instantiate(rock, position, new Quaternion(), transform);
        spawnedBlock.transform.localScale *= randomSize;

        Health health = spawnedBlock.GetComponent<Health>();
        if(health != null) health.AddHealth((int)randomSize);

        rb = spawnedBlock.GetComponent<Rigidbody>();
        rb.mass *= randomSize;
    }
    float Randomize(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

}

