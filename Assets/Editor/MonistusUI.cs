using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class LattiaMonistusUI : MonoBehaviour
{
    [MenuItem("tools/multiply")]
    private static void Monista()
    {
        Monistus monistus = FindObjectOfType<Monistus>();
        if (monistus != null)
        {
            UnityEngine.Debug.LogError("Monistus-skripti� on l�ytynyt!");
            monistus.Monista();
        }
        else
        {
            UnityEngine.Debug.LogError("Monistus-skripti� ei l�ytynyt!");
        }
    }
}
