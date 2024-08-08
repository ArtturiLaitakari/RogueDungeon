using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 10f; // Py�rimisnopeus asteina per sekunti
    public Vector3 rotationAxis = Vector3.up; // Py�rimisakseli, oletuksena y-akseli

    void Update()
    {
        // Py�rit� GameObjectia aikav�lin mukaisesti
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
