using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame() called - this only works in a built game.");
    }
}
