using UnityEngine;

/// <summary>
/// Parent class for all projectiles.
/// </summary>
public class Projectile : MonoBehaviour
{
    private const int D = 1;
    public float speed;
    public float time;
    protected Rigidbody rb;
    protected float t;
    public AudioSource audioSource;
    public string shooterTag = "Player";
    public int damage = D;

    /// <summary>
    /// A projectile has been created, make it fast
    /// </summary>
    void Start()
    {
        t = time;
        Vector3 playerForward = transform.forward;
        playerForward.y = 0; 
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
        rb = GetComponent<Rigidbody>();
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
    protected virtual void OnTriggerEnter(Collider other)    
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
