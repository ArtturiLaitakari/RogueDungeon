using System;
using UnityEngine;

/// <summary>
/// Melee attack class for the player
/// </summary>

public class Attack : MonoBehaviour
{
    public string shooterTag="Player";
    public float meleeDelay; 
    public Animator animator;
    private float lastHitTime = 0;
    public int damage = 1;

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
            rb.isKinematic = true; // Varmistaa, ett‰ objekti ei putoa
        }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (animator == null) throw new Exception("Lis‰‰ animator");
        if (other.CompareTag("muzzle") || other.CompareTag(shooterTag)) return;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        string clipName = clipInfo[0].clip.name;

        if (clipName.StartsWith("Attack") && currentState.normalizedTime < 1.0f)
        {
            CheckForDamage(other);
        }
    }

    /// <summary>
    /// Attack causes damage to the collied target
    /// </summary>
    /// <param name="other"></param>

    private void CheckForDamage(Collider other)
    {
        // Tarkista, onko kulunut tarpeeksi aikaa viimeisimm‰st‰ osumasta
        if (Time.time - lastHitTime >= meleeDelay)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.ReduceHealth(damage);
            }
        }
    }
}
