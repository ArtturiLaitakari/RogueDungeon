using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 10f; // Pyörimisnopeus asteina per sekunti
    public Vector3 rotationAxis = Vector3.up; // Pyörimisakseli, oletuksena y-akseli

    void Update()
    {
        // Pyöritä GameObjectia aikavälin mukaisesti
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
