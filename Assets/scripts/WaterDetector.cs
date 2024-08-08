using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    public PlayerControllers playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            playerController.Immersed(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            playerController.Immersed(false);
        }
    }
}
