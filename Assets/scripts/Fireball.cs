using UnityEngine;

/// <summary>
/// behaviour of Fireball and Iceball
/// </summary>
public class Fireball : Projectile
{
    public float radius;
    public GameObject explosion;

    /// <summary>
    /// Counter for coming explosion
    /// </summary>
    public override void Update()
    {
        t -= Time.deltaTime;
        if (t < 0 ) {
            Explode();
        }
    }

    /// <summary>
    /// Handle explosion, cause damage to all colliders around
    /// </summary>
    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for(int i = 0; i < colliders.Length; i++)
        {
            Health health = colliders[i].GetComponent<Health>();
            if(health != null)
            {
                health.ReduceHealth(damage);
            }
        }
        Instantiate(explosion, transform.position, new Quaternion());
        Destroy(gameObject);
    }

    /// <summary>
    /// On hit, explode, dismiss muzzle and self
    /// </summary>
    /// <param name="other">target hit</param>
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("muzzle") && !other.CompareTag(shooterTag))
        {
            Explode();
        }
    }

}
