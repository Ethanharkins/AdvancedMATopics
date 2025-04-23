using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void SetLanguageEnglish()
    {
        LanguageManager.SelectedLanguage = "English";
        Debug.Log("Language set to English");
    }

    public void SetLanguageSpanish()
    {
        LanguageManager.SelectedLanguage = "Spanish";
        Debug.Log("Language set to Spanish");
    }

    public void StartGame(string gameplaySceneName)
    {
        if (string.IsNullOrEmpty(LanguageManager.SelectedLanguage))
        {
            Debug.LogWarning("No language selected! Defaulting to English.");
            LanguageManager.SelectedLanguage = "English";
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
