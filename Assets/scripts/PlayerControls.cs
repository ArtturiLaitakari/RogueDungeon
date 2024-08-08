using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControllers : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    public float movementSpeed;
    private float turningSpeed;
    public float manaRestore;

    public Transform muzzle;
    public float multiplier=200f;

    private float meleeDamageTime;
    private float defenseMana = 2;
    private float Velocity;
    private bool isMovingForward = true;
    private float manaTimer;
    private float fatiqueTimer=0;
    private float drowningTimer = 0;
    private float tMelee;
    private float currentSpeed=0;
    private Health health;
    public bool melee = false;
    public ISpells spells;
    private Attack attack;
    private float meleeAnimationTime=1.2f;
    private Renderer characterRenderer;
    private int scale=2;
    private bool isInWater = false;
    private Vector2 moveInputValue;

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

        if (Mathf.Abs(inputHorizontal) < 0.2f)
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
        float rotationSpeed = turningSpeed * multiplier;

        if (moveInputValue.magnitude > 0.1f)
        {
            Vector3 movement = new Vector3(moveInputValue.x, 0.0f, moveInputValue.y);
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            MoveAnimation(1f, true);
        }
        else
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
        Move_Vertical(moveInputValue.y);
        Move_Horizontal(moveInputValue.x);
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
            if (manaTimer <= 0) GameController.instance.ShowMana(0);
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
    /// Calculates players movement and activates animations.
    /// </summary>
    void FixedUpdate()
    {
        //moveInputValue = Vector2.zero;
        //ReadKeyboard();
        if (GameController.instance.Isometric) WorldAxisMovement();
        else RelativeAxisMovement();
        if (rb.position.y < -7)
        {
            health.ReduceHealth(1);
        }
        if (isInWater)
        {
            isDrowning();
        } else if(drowningTimer < 0)
        {         
            drowningTimer = 0;
        }
    }

    /// <summary>
    /// Checks the drowning timer and applies fatigue or reduces health
    /// based on the elapsed drowning time. The method uses predefined
    /// thresholds to determine when to apply fatigue or health reduction.
    /// </summary>
    private void isDrowning()
    {
        drowningTimer -= Time.deltaTime;
        GameController.instance.ShowMana("Magic inactive");
        GameController.instance.WaterAudio();

        // Taulukko ajastimille ja niihin liittyville toiminnoille
        Dictionary<float, Action> drowningActions = new Dictionary<float, Action>
        {
            { -10, health.AddFatique },
            { -20, health.AddFatique },
            { -30, health.ReduceHealth },
            { -40, health.ReduceHealth }
        };

        foreach (var action in drowningActions)
        {
            if (drowningTimer < action.Key)
            {
                action.Value.Invoke();
                // Prevent the same action from being invoked multiple times
                drowningTimer = Mathf.Max(drowningTimer, action.Key);
            }
        }
    }

    public void Immersed(bool immersed)
    {
        isInWater = immersed;
        if(isInWater)
        {
            GameController.instance.ShowMana("Magic inactive");
            GameController.instance.SetCondition("Drowning");
        }
        else
        {
            GameController.instance.ShowMana("Mana: 0");
            GameController.instance.SetCondition("");
        }
    }

    private void OnMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
    }

    private void ReadKeyboard()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) moveInputValue.x = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) moveInputValue.x = 1f;

        if (Input.GetKey(KeyCode.UpArrow)) moveInputValue.y = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveInputValue.y = -1f;
    }

    /// <summary>
    /// Show Mana score and tell if its spell casting or refreshing.
    /// </summary>
    private void ShowMana()
    {
        GameController.instance.ShowMana((float)Math.Round(manaTimer, 1));
    }


    /// <summary>
    /// Instantiates and fires a projectile from the muzzle's position and rotation.
    /// the mana restore timer is reset. 
    /// </summary>
    private void FireProjectile()
    {
        if (isInWater) // and magic attack
        {
            GameController.instance.ShowMana("magic inactive");
        }
        else
        {
            spells.AttackSpell();
            manaTimer = manaRestore;
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
        if (isInWater)
        {
            GameController.instance.ShowMana("magic inactive");
            return false;
        }      
        if (health.GetFatique() > 2)
        {
            GameController.instance.ShowMana("Too tired.");
            return false;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.gameObject.tag;
        if (colliderTag == "Finish")
        {
            GameController.instance.NextLevel();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            GameController.instance.ShowMana("Mana: 0");
        }
    }
}
