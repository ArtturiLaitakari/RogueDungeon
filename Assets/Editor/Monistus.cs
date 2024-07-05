using UnityEngine;

public class Monistus : MonoBehaviour
{
    public GameObject laattaPrefab;

    public int laattojenMaaraX = 4;
    public int laattojenMaaraZ = 4;
    public Vector3 tileSize;

    public void Monista()
    {
        UnityEngine.Debug.Log(tileSize);
        for (int x = 0; x < laattojenMaaraX; x++)
        {
            for (int z = 0; z < laattojenMaaraZ; z++)
            {
                Vector3 sijainti = new Vector3(x * tileSize.x, 0f, z * tileSize.z);
                Instantiate(laattaPrefab, sijainti, Quaternion.identity);
            }
        }
    }
}
