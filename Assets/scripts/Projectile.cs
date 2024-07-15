using UnityEngine;

/// <summary>
/// Parent class for all projectiles.
/// </summary>
public class Projectile : Attack
{
    public float speed;
    public float time;
    protected Rigidbody rb;
    protected float t;
    public AudioSource audioSource;

    /// <summary>
    /// A projectile has been created, make it fast
    /// </summary>
    void Start()
    {
        t = time;
        rb = GetComponent<Rigidbody>();
        Vector3 playerForward = transform.forward;
        playerForward.y = 0; 
        rb.velocity = playerForward * speed;
    }

    /// <summary>
    /// Give it a lifespan
    /// </summary>

    public virtual void Update()
    {
        t -= Time.deltaTime;
        if (t < 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// when projectile hits, it damages the target
    /// then projectile is destroyed
    /// </summary>
    /// <param name="other">target</param>
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("muzzle") && !other.CompareTag(shooterTag))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.ReduceHealth(damage);
            }
            Destroy(gameObject);
        }
    }
}
