using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerControllers : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    public float movementSpeed;
    public float turningSpeed;
    public float manaRestore;

    public GameObject fireball;
    public Transform muzzle;

    private float Velocity;
    private bool isMovingForward = true;
    private float t;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        t = 0f;
    }

    /// <summary>
    /// This method handles the firing of a projectile (fireball) when the "Fire1" button is pressed.
    /// It checks if the mana cooldown (t) is zero or not. If the cooldown is zero, it instantiates
    /// a new fireball projectile at the muzzle position and sets the shooter's tag. Otherwise, it
    /// decreases the cooldown timer.
    /// </summary>
    private void Update()
    {
        if (t <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject projectile = Instantiate(fireball, muzzle.position, muzzle.rotation);                
                projectile.GetComponent<Fireball>().shooterTag = tag;
                t = manaRestore;
            }
        } else
        {
            t -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Handles fixed physics updates. Calculates player movement and activates animations.
    /// </summary>
    void FixedUpdate()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        if (inputHorizontal != 0)
        {
            Vector3 turning = Vector3.up * inputHorizontal;
            rb.angularVelocity = turning * turningSpeed;
        }
        else rb.angularVelocity = new Vector3();
        if (inputVertical != 0)
        {
            Vector3 movement = transform.forward * inputVertical;
            isMovingForward = inputVertical > 0;
            rb.velocity = movement * movementSpeed;
            ActivateAnimation(rb.velocity.magnitude, isMovingForward);
        }
        else ActivateAnimation(0);
        if(rb.position.y < -10)
        {
            var health = GetComponent<Health>();
            health.ReduceHealth(1);
        }
    }

    /// <summary>
    /// Activates the player's animation based on movement intensity.
    /// </summary>
    /// <param name="movement">The intensity of movement (0 to 1).</param>
    /// <param name="isForward">Whether the movement is forward (true) or backward (false).</param>
    void ActivateAnimation(float movement, bool isForward=true)
    { 
        this.Velocity = Mathf.Clamp01(movement);
        animator.SetFloat("Velocity", this.Velocity);
        animator.SetFloat("Speed", isForward ? 1: -1); // Muuta Animation Speed -parametri
    }
}
