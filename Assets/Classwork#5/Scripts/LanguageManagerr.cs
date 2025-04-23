using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static string SelectedLanguage = "English"; // Default language

    public void SetLanguageEnglish()
    {
        SelectedLanguage = "English";
        Debug.Log("Language set to English");
    }

    public void SetLanguageSpanish()
    {
        SelectedLanguage = "Spanish";
        Debug.Log("Language set to Spanish");
    }

    public void StartGame(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
