using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinZoneTrigger : MonoBehaviour
{
    public GameObject winUI;

    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log(" Player entered Win Zone!");

            hasWon = true;
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (winUI != null)
            {
                winUI.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null); // Fix input focus
            }

            // Optionally disable player movement
            NewPlayerController playerController = other.GetComponent<NewPlayerController>();
            if (playerController != null)
                playerController.enabled = false;
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log(" Quit Game");
        Application.Quit();
    }
}
