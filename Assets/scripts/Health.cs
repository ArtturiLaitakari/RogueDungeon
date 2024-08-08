using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Health manager for characters.
/// </summary>
public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public GameObject splintering;
    public Color damageColor = Color.red;
    public float damageFlashTime = 1f;

    public int currentHealth=3;
    public int fatiqueLevel = 0;
    private Color originalColor;
    private Color fadingColor;
    private float t;
    private MeshRenderer[] meshRenderers;
    public bool dead = false;

    public int GetFatique() => fatiqueLevel;
    public AudioSource audioSource;

    void Start()
    {
        meshRenderers = FindAllMeshRenderers(gameObject).ToArray();

        if (meshRenderers.Length > 0)
        {
            originalColor = meshRenderers[0].material.color;
        }
        else
        {
            originalColor = Color.white;
        }

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) throw new Exception("puuttuu" + gameObject.name);
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.loop = false;
    }

    /// <summary>
    /// Sets character Health, current and max
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    public void Sethealth(int current, int max)
    {
        currentHealth = current;
        maxHealth = max;
    }

    /// <summary>
    /// Handle when character takes damage
    /// </summary>
    /// <param name="damage">damage amount</param>
    public void ReduceHealth(int damage)
    {
        StartCoroutine(DamageFlash());
        currentHealth -= damage;
        if (gameObject.tag == "Player")
            GameController.instance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            if (gameObject.tag == "Enemy") GameController.instance.EnemyDestroyed();
            Instantiate(splintering, transform.position, new Quaternion());
            Destroy(gameObject);
        } else if(!dead) audioSource.Play();

        // may happen, should not
        if (currentHealth < 0 && gameObject != null)
        {
            dead = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Reduces the player's health by the default amount (1).
    /// </summary>
    public void ReduceHealth()
    {
        ReduceHealth(1);
    }

    /// <summary>
    /// Coroutine that flashes the object with a damage color for a specified duration.
    /// </summary>
    /// <returns>Returns an IEnumerator that can be used to control the coroutine.</returns>
    private IEnumerator DamageFlash()
    {
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

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public int GetWounds() => (maxHealth - currentHealth);
    public void AddFatique() => fatiqueLevel++;
    public void HealFatique() 
    {
        if (fatiqueLevel > 0) fatiqueLevel--;
    }

    public void AddHealth(int h=1) 
    {
        if (currentHealth < maxHealth) currentHealth += h;
    }

    public int MaxHealth() => maxHealth;
}