using UnityEngine;

/// <summary>
/// NPC melee attack class
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    protected int damage=1; // we use delay to determine damage, not this, but still avoid numbers.
    public string shooterTag="Enemy";
    public float meleeDelay; 
    private float lastHitTime=0;

    void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Varmistaa, että objekti ei putoa
        }
    }

    /// <summary>
    /// NPC attack method, cause damage upon hit.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("muzzle") && !other.CompareTag(shooterTag))
        {
            if (Time.time - lastHitTime >= meleeDelay)
            {
                Health health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.ReduceHealth(damage);
                }
                // Päivitä viimeisimmän osuman aika
                lastHitTime = Time.time;
            }
        }
    }
}
