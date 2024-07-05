using System.Security.Cryptography;
using UnityEngine;

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

    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            target = player.transform;
        }
        else
        {
            // Aseta kameran sijainti pelaajan sijainnin mukaan

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
            if (isometric)
            {
                transform.rotation = Quaternion.Euler(isometricAngle, transform.rotation.y, transform.rotation.z);
            }

        }
    }

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
