using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float time;
    public float radius;
    public int damage;

    public string shooterTag;
    public GameObject explosion;

    private Rigidbody rb;
    private float t;

    void Start()
    {
        t = time;
        rb = GetComponent<Rigidbody>();
        Vector3 playerForward = transform.forward;
        playerForward.y = 0; // Aseta y-akseli nollaksi
        rb.velocity = playerForward * speed;
    }

    void Update()
    {
        t -= Time.deltaTime;
        if (t < 0 ) {
            Explode();
        }
    }
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
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("muzzle") && !other.CompareTag(shooterTag))
        {
            Explode();
        }
    }

}
