using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    public PlayerControllers playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            playerController.Immersed(true);
            GameController.instance.WaterAudio();
            if (!GameController.instance.Isometric) RenderSettings.fog = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            playerController.Immersed(false);
            GameController.instance.WaterAudio();
            if (!GameController.instance.Isometric) RenderSettings.fog = true;
        }
    }
}
