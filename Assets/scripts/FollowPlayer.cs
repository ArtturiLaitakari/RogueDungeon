using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// Follow players camera controller
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    public Transform target; // Aseta tämä kameran seuraaman pelaajan Transform
    public Vector3 offset; // Aseta haluamasi etäisyys kameran ja pelaajan välille
    public bool isometric = true;
    public float isometricAngle;
    public float lookAtAroundAngle =180;
    public float cameraShift = 2f;
    public float smoothTime = 0.3f;
    private int shift = 3;
    private float distance = 3.4f;
    private float y = 2.3f;
    private Vector3 velocity = Vector3.zero;
    private Vector2 moveInputValue;
    private GameObject player;

    /// <summary>
    /// LateUpdate is called every frame, after all Update functions have been called.
    /// It is used here to update the camera position and orientation based on the 
    /// players's position and user input for toggling between 
    /// isometric and third-person views.
    /// </summary>
    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }
        else
        {
            if (!isometric)
            {
                ThirdPersonUpdate();
            }
            else
            {
                IsometricUpdate();
            }
        }

        if (Input.GetButtonDown("ToggleView"))
        {
            Debug.Log("ToggleView");
            isometric = !isometric;
            GameController.instance.Isometric = isometric;
            if (isometric)
            {
                transform.rotation = Quaternion.Euler(isometricAngle, transform.rotation.y, transform.rotation.z);
            }
        }
    }
    private void OnMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
    }

    private void IsometricUpdate()
    {
        Vector3 newOffset = offset;
        if (moveInputValue.y > 0.1f)
        {
            newOffset.z = offset.z + cameraShift * shift;
        }
        if (moveInputValue.y < -0.5f)
        {
            newOffset.z = offset.z - cameraShift * shift;
            newOffset.y = offset.y - cameraShift;
        }
        if (moveInputValue.x > 0.5f)
        {
            newOffset.x = offset.x + cameraShift * shift;
        }
        if (moveInputValue.x < -0.5f)
        {
            newOffset.x = offset.x - cameraShift * shift;
        }

        Vector3 targetPosition = target.position + newOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    /// <summary>
    /// Updates the camera position and rotation in third-person view based on 
    /// the players's position and orientation.
    /// </summary>
    private void ThirdPersonUpdate()
    {
        if (target == null) { return; }

        Quaternion currentRotation = target.rotation;
        Vector3 position = target.position;
        position.y = target.position.y + y;
        transform.position = position - target.forward * distance;
        transform.rotation = currentRotation;
    }
}
