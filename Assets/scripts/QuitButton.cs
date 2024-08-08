using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Editorissa k‰ytet‰‰n t‰t‰ metodia pelin pys‰ytt‰miseen
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Build-versiossa k‰ytet‰‰n t‰t‰ metodia pelin lopettamiseen
        Application.Quit();
#endif
    }
}
