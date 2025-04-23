using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Exiting the game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Works in editor
#else
        Application.Quit(); // Works in build
#endif
    }
}
