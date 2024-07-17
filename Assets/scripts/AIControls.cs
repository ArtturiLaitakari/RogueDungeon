using System.Diagnostics;
using UnityEngine;

public class AIControllers : MonoBehaviour
{
    private Rigidbody rb;
    public float movementSpeed;
    public float turningSpeed;
    public float attackDelay=2;

    public float detectRange = 15f;
    public float attackRange=10f;
    public float stoppingRange = 3f;
    public float switchTargetRange = 2f;
    public float switchDistance = 10f;
    public float AIDelay = 0.5f;

    public GameObject projectile;
    public Transform muzzle;
    public bool melee = false;
    public IAnimationController animationController;

    private float Velocity;
    private bool isMovingForward = true;
    private float AIt;
    private float Pt;

    private GameObject targetObject;
    private Vector3 target;

    private int obstacleMask;

    private enum State { forward, left, right, back, stop };
    private State state;
    private State nextState;

    void Start()
    {
        rb = GetComponent <Rigidbody> ();
        AIt = 0f;
        Pt = 0f;
        obstacleMask = LayerMask.GetMask("Obstacle");
        state = State.forward;
        nextState = State.forward;
        animationController = GetComponent<IAnimationController> ();
    }

    /// <summary>
    /// Handles fixed physics updates. Calculates players movement and activates animations.
    /// </summary>
    void FixedUpdate()
    {
        FindPlayer();

        if (AIt < 0)
        {
            state = nextState;
            AIt = AIDelay;
        }
        else
        {
            AIt -= Time.deltaTime;
        }
        StateMachine(state);
        if (rb.position.y < -10)
        {
            var health = GetComponent<Health>();
            health.ReduceHealth(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.gameObject.tag;
        if (colliderTag != "Obstacle" && colliderTag != "wall") return;

        RaycastHit leftHit;
        RaycastHit rightHit;

        float leftLength = CalculateRayLength(transform.forward + transform.right * -1, out leftHit);
        float rightLength = CalculateRayLength(transform.forward + transform.right, out rightHit);

        if (leftLength > rightLength)
        {
            state = State.left;
            target = leftHit.point;
        }
        else {
            state = State.right;
            target = rightHit.point;
        }
    }

    /// <summary>
    /// Calculates the length of a ray in the specified direction.
    /// </summary>
    /// <param name="rayDirection">The direction of the ray.</param>
    /// <param name="hit">The RaycastHit object to store the hit information.</param>
    /// <returns>The length of the ray if it hits an obstacle, otherwise 0.</returns>
    private float CalculateRayLength(Vector3 rayDirection, out RaycastHit hit)
    {
        var rayhit = Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, obstacleMask) ? hit.distance : 0f;
        return rayhit;
    }

    /// <summary>
    /// Called when the Collider other has stopped touching the trigger.
    /// Sets the next state to 'forward'.
    /// </summary>
    /// <param name="other">The Collider that has exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        nextState = State.forward;
    }

    /// <summary>
    /// Called when this collider/rigidbody has begun touching another rigidbody/collider.
    /// Sets the state to 'back' if the collision is with an obstacle or wall.
    /// </summary>
    /// <param name="collision">The Collision data associated with this collision event.</param>

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag != "Obstacle" && collisionTag != "wall") return;

        state = State.back;
    }

    /// <summary>
    /// Controls the behavior of the object based on its current state.
    /// </summary>
    /// <param name="state">The current state of the object.</param>
    private void StateMachine(State state)
    {
        // liikutaan pelaajaa kohti
        float angle = Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);

        if (state == State.forward)
        {
            if (angle < 0)
            {
                Turning(-1f);
            }
            else if (angle > 0)
            {
                Turning(1f);
            }

            if (Mathf.Abs(angle) < 90)
            {
                Move(1f);
            }
        }
        if (state == State.left)
        {
            Turning(-1f);
            Move(1f);

        }
        if (state == State.right)
        {
            Turning(1f);
            Move(1f);
        }
        if (state == State.back)
        {
            Move(-1f);
            nextState = State.forward;
        }
        if (state == State.stop)
        {
            Move(0f);
            nextState = State.forward;
        }
    }
    /// <summary>
    /// find target == players
    /// </summary>
    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, target) < switchTargetRange)
        {
            float randomX = UnityEngine.Random.Range(-switchDistance, switchDistance);
            float randomZ = UnityEngine.Random.Range(-switchDistance, switchDistance);
            target += new Vector3(randomX, 0f, randomZ);
        }
        Pt -= Time.deltaTime;
        if (targetObject != null)
        {
            if(Vector3.Distance(transform.position, targetObject.transform.position) < detectRange) { // range of visibility
              if (!Physics.Linecast(transform.position, targetObject.transform.position, obstacleMask)) // line of visibility
              {
                target = targetObject.transform.position;

                if (Vector3.Distance(target, transform.position) < attackRange)
                {
                    // Hyökkää vasta, kun etäisyys on pienempi kuin attackRange
                    if (Pt < 0)
                    {
                        if (!melee) FireProjectile();
                        else MeleeAttack(); 
                    }
                }
                if (Vector3.Distance(target, transform.position) < stoppingRange )
                {
                    nextState = State.stop;
                }
              }
            }
        }
        else
        {
            targetObject = GameObject.FindWithTag("Player");
        }
    }



    /// <summary>
    /// Instantiates a projectile at the muzzle position and rotation.
    /// </summary>
    void FireProjectile()
    {
        GameObject p = Instantiate(projectile, muzzle.position, muzzle.rotation);
        p.GetComponent<Fireball>().shooterTag = tag;
        Pt = attackDelay;
    }
    /// <summary>
    /// Activate attack melee animation
    /// </summary>
    void MeleeAttack()
    {
        Pt = attackDelay;
        animationController.Attack();
    }

    /// <summary>
    /// Activates the players's animation based on movement intensity.
    /// </summary>
    /// <param name="movement">The intensity of movement (0 to 1).</param>
    /// <param name="isForward">Whether the movement is forward (true) or backward (false).</param>
    void MoveAnimation(float movement, bool isForward = true)
    {
        this.Velocity = Mathf.Clamp01(movement);
        animationController.Move(this.Velocity, isForward);
    }

    /// <summary>
    /// Moves the object based on the input value.
    /// </summary>
    /// <param name="input">The movement input, where positive values move the object forward and negative values move it backward.</param>
    private void Move(float input)
    {
        Vector3 movement = transform.forward * input;
        isMovingForward = input > 0;
        rb.velocity = movement * movementSpeed;
        MoveAnimation(rb.velocity.magnitude, isMovingForward);
    }

    /// <summary>
    /// Rotates the object based on the input value.
    /// </summary>
    /// <param name="input">The turning input, where positive values rotate the object to the right and negative values rotate it to the left.</param>
    private void Turning(float input)
    {
        Vector3 turning = Vector3.up * input;
        rb.angularVelocity = turning * turningSpeed;
    }
}
