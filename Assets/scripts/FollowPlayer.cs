using UnityEngine;

/// <summary>
/// Follow player camera controller
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    private Transform target; // Aseta tämä kameran seuraaman pelaajan Transform

    public Vector3 offset; // Aseta haluamasi etäisyys kameran ja pelaajan välille
    public bool isometric = true;
    public float isometricAngle;
    public float lookAtAroundAngle =180;
    public float distance=2f;
    public float y = 1.5f;

    private GameObject player;

    /// <summary>
    /// LateUpdate is called every frame, after all Update functions have been called.
    /// It is used here to update the camera position and orientation based on the 
    /// player's position and user input for toggling between 
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
                transform.position = target.position + offset;
            }
        }
        if (Input.GetButtonDown("ToggleView"))
        {
            isometric = !isometric;
            GameController.instance.Isometric = isometric;
            if (isometric)
            {
                transform.rotation = Quaternion.Euler(isometricAngle, transform.rotation.y, transform.rotation.z);
            }

        }
    }

    /// <summary>
    /// Updates the camera position and rotation in third-person view based on 
    /// the player's position and orientation.
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
