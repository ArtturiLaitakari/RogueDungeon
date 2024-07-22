using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerControllers : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    public float movementSpeed;
    private float turningSpeed;
    public float manaRestore;

    public Transform muzzle;
    public AudioSource audiosource;
    public float multiplier=200f;

    private float meleeDamageTime;
    private float defenseMana = 2;
    private float Velocity;
    private bool isMovingForward = true;
    private float manaTimer;
    private float fatiqueTimer=0;
    private float tMelee;
    private float currentSpeed=0;
    private Health health;
    public bool melee = false;
    public ISpells spells;
    private Attack attack;
    private float meleeAnimationTime=1.2f;
    private Renderer characterRenderer;
    private int scale=2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        manaTimer = 0f;
        var characterAbilities = GetComponent<Abilities>();
        movementSpeed = characterAbilities.Agility*scale;
        turningSpeed = characterAbilities.Agility;
        manaRestore = 4-characterAbilities.Power;
        meleeDamageTime = 4 - characterAbilities.Strength;
        currentSpeed = movementSpeed;
        int hits = characterAbilities.Strength + 2;
        health = GetComponent<Health>();
        spells = GetComponent<ISpells>();
        health.Sethealth (hits, hits);
        GameController.instance.SetAbilities(characterAbilities, spells);
        GameController.instance.SetHealth (hits, hits);
        GameController.instance.SetFatique(0);
        characterRenderer = GetComponent<Renderer>();

        if (melee)
        {
            attack = GetComponentInChildren<Attack>();
            attack.meleeDelay = meleeDamageTime;
        }
    }

    /// <summary>
    /// shooter mode move vertical
    /// </summary>
    /// <param name="inputVertical"></param>
    private void Move_Vertical(float inputVertical)
    {
        Vector3 movement = transform.forward * inputVertical;
        isMovingForward = inputVertical > 0;
        rb.velocity = movement * currentSpeed;
        rb.velocity = new Vector3(movement.x * currentSpeed, rb.velocity.y, movement.z * currentSpeed);
        MoveAnimation(rb.velocity.magnitude, isMovingForward);
    }

    /// <summary>
    /// shooter mode horizontal movement
    /// </summary>
    /// <param name="inputHorizontal"></param>
    private void Move_Horizontal(float inputHorizontal)
    {

        if (Mathf.Abs(inputHorizontal) < 0.1f)
        {
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Vector3 turning = Vector3.up * inputHorizontal;
            rb.angularVelocity = turning * turningSpeed;
        }
    }

    /// <summary>
    /// Isometric movement
    /// </summary>
    private void WorldAxisMovement()
    {
        float dpadHorizontal = Input.GetAxis("Horizontal");
        float dpadVertical = Input.GetAxis("Vertical");
        float rotationSpeed = turningSpeed * multiplier;
        if (Mathf.Abs(dpadHorizontal) > 0.1f || Mathf.Abs(dpadVertical) > 0.1f)
        {
            Vector3 movement = new Vector3(dpadHorizontal, 0.0f, dpadVertical);
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime );
            MoveAnimation(1f, true);
        } else
        {
            MoveAnimation(0f, true);
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Shooter mode movement
    /// </summary>
    private void RelativeAxisMovement()
    {
        float dpadHorizontal = Input.GetAxis("Horizontal");
        float dpadVertical = Input.GetAxis("Vertical");
        // forward
        Move_Vertical(dpadVertical);
        Move_Horizontal(dpadHorizontal);
    }

    /// <summary>
    /// This method handles the firing of a projectile (fireball) when the "Fire1" button is pressed.
    /// It checks if the mana cooldown (manaTimer) is zero or not. If the cooldown is zero, it instantiates
    /// a new fireball projectile at the muzzle position and sets the shooter's tag. Otherwise, it
    /// decreases the cooldown timer.
    /// </summary>
    private void Update()
    {
        if (tMelee <= 0 && melee)
        {
            if (Input.GetButtonDown("Fire1") && melee)
            {
                MeleeAttack();
                tMelee = meleeAnimationTime;
            }
        }
        if (tMelee > 0 && melee)
        {
            tMelee -= Time.deltaTime;
        }

        if (manaTimer <= 0)
        {
            if (Input.GetButtonDown("Fire1") && !melee)
            {
                FireProjectile();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                if (DefenseSpell())
                {
                    health.AddFatique();
                    currentSpeed = movementSpeed - health.GetFatique();
                    GameController.instance.SetFatique(health.GetFatique());
                }
            }
        }
        else
        {
            manaTimer -= Time.deltaTime;
            fatiqueTimer = manaRestore * defenseMana;
            if (manaTimer <= 0) GameController.instance.ShowMana(0, 1);
            else ShowMana();
        }
        if (health.GetFatique() > 0)
        {
            if (fatiqueTimer <= 0)
            {
                health.HealFatique();
                currentSpeed = movementSpeed - health.GetFatique();
                GameController.instance.SetFatique(health.GetFatique());
                fatiqueTimer = manaRestore * defenseMana;
            }
            else
            {
                fatiqueTimer -= Time.deltaTime;
            }
        } else if (fatiqueTimer < 0 ) GameController.instance.SetFatique(health.GetFatique());


    }

    /// <summary>
    /// Show Mana score and tell if its spell casting or refreshing.
    /// </summary>
    private void ShowMana()
    {
        string action = "";
        if (health.GetFatique() == 0) action = "refreshing";
        else action = spells.GetDefenseSpellName();
        GameController.instance.ShowMana((float)Math.Round(manaTimer, 1), 
            manaRestore, action);
    }


    /// <summary>
    /// Instantiates and fires a projectile from the muzzle's position and rotation.
    /// the mana restore timer is reset. An audio effect is also played.
    /// </summary>
    private void FireProjectile()
    {
        spells.AttackSpell();
        manaTimer = manaRestore;
        audiosource.Play();
    }

    /// <summary>
    /// Calculates players movement and activates animations.
    /// </summary>
    void FixedUpdate()
    {
        if (GameController.instance.Isometric) WorldAxisMovement();
        else RelativeAxisMovement();
        if (rb.position.y < -10)
        {
            health.ReduceHealth(1);
        }
    }

    /// <summary>
    /// Activates the players's animation based on movement intensity.
    /// </summary>
    /// <param name="movement">The intensity of movement (0 to 1).</param>
    /// <param name="isForward">Whether the movement is forward (true) or backward (false).</param>
    private void MoveAnimation(float movement, bool isForward=true)
    {
        this.Velocity = Mathf.Clamp01(movement);
        animator.SetFloat("Velocity", movement);
        animator.SetFloat("Animation Speed", isForward ? 1: -1); // Muuta Animation Speed -parametri
    }

    /// <summary>
    /// Heals the players by increasing their health by 1 unit. 
    /// Adjusts the players's current speed based on the new health status, 
    /// factoring in any fatigue.
    /// </summary>
    private bool DefenseSpell()
    {
        if (health.GetFatique() > 2) return false;
        if (spells.DefenseSpell())
        {
            manaTimer = manaRestore * defenseMana;
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Activate Melee attack animation
    /// </summary>
    private void MeleeAttack()
    {
        animator.SetFloat("Animation Speed", 0.5f);
        animator.SetTrigger("Attack");
    }
}
