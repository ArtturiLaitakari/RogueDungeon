using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth=3;
    public GameObject splintering;
    public Color damageColor = Color.red;
    public float damageFlashTime = 1f;

    private int currentHealth=3;
    private int woundCount; // serious damage slows character down, TODO
    private Color originalColor;
    private Color fadingColor;
    private float t;
    private MeshRenderer[] meshRenderers;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        // Käytä rekursiivista funktiota kaikkien MeshRenderer-objektien löytämiseen
        meshRenderers = FindAllMeshRenderers(gameObject).ToArray();

        if (meshRenderers.Length > 0)
        {
            originalColor = meshRenderers[0].material.color;
        }
        else
        {
            Debug.LogWarning("No MeshRenderer found in the hierarchy.");
        }

        originalColor = meshRenderers[0].material.color;
    }

    public void ReduceHealth(int damage)
    {
        StartCoroutine(DamageFlash());
        currentHealth -= damage;
        if (gameObject.tag == "Player") GameController.instance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            if (gameObject.tag == "Enemy") GameController.instance.EnemyDestroyed();
            Instantiate(splintering, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        // Aseta väri damageColoriksi kerran ennen silmukkaa

        float t = damageFlashTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadingColor = Color.Lerp(originalColor, damageColor, t / damageFlashTime);
            ResetMeshColors(meshRenderers, fadingColor);
            yield return null;
        }
        ResetMeshColors(meshRenderers, originalColor);
    }

    /// <summary>
    /// Resets the color and emission color of all MeshRenderer objects in the provided collection to the specified original color.
    /// </summary>
    /// <param name="meshRenderers">A collection of MeshRenderer objects whose colors will be reset.</param>
    /// <param name="originalColor">The original color to which the MeshRenderer colors will be reset.</param>
    private void ResetMeshColors(IEnumerable<MeshRenderer> meshRenderers, Color originalColor)
    {
        foreach (MeshRenderer r in meshRenderers)
        {
            r.material.color = originalColor;
        }
    }

    /// <summary>
    /// Recursively finds all MeshRenderer components in the given GameObject and its children.
    /// </summary>
    /// <param name="gameObject">The root GameObject to search.</param>
    /// <returns>A list of all MeshRenderer components found.</returns>
    private List<MeshRenderer> FindAllMeshRenderers(GameObject gameObject)
    {
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        // Add MeshRenderer from the current GameObject if it exists
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderers.Add(meshRenderer);
        }

        // Recursively add MeshRenderers from the children
        foreach (Transform child in gameObject.transform)
        {
            meshRenderers.AddRange(FindAllMeshRenderers(child.gameObject));
        }

        return meshRenderers;
    }


    public int ShowHealth() => currentHealth;

    public void SetHealth(int h) => currentHealth = h;

    public int GetHealth() => currentHealth;

    public void AddHealth(int h) => currentHealth += h;

    public int MaxHealth() => maxHealth;

}