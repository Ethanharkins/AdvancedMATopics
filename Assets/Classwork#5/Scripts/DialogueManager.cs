using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Files")]
    public TextAsset englishInkJSON;
    public TextAsset spanishInkJSON;

    [Header("UI Elements")]
    public TMP_Text subtitleText;
    public GameObject dialogueCanvas;
    public GameObject nextButton;
    public Button[] choiceButtons;

    [Header("Cameras")]
    public GameObject cameraMain;
    public GameObject cameraA;
    public GameObject cameraB;

    [Header("Player Control")]
    public MonoBehaviour playerMovementScript;

    [Header("Audio")]
    public AudioSource voicePlayer;
    private Dictionary<string, AudioClip> voiceClips = new Dictionary<string, AudioClip>();
    private Coroutine waitForVoiceCoroutine;

    // Spanish voice tracking
    private int aLineCount = 0;
    private Dictionary<string, string> aLineIndexMap = new Dictionary<string, string>();

    private Story currentStory;
    private string currentLanguage = "English";
    private bool dialogueStarted = false;
    private bool waitingForClick = false;

    void Start()
    {
        cameraMain.SetActive(true);
        if (cameraA != null) cameraA.SetActive(false);
        if (cameraB != null) cameraB.SetActive(false);

        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            Debug.Log("Dialogue canvas deactivated at start.");
        }

        nextButton.SetActive(false);
        HideAllChoices();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        LoadVoiceClips();
    }

    public void StartDialogue()
    {
        if (dialogueStarted) return;
        dialogueStarted = true;

        currentLanguage = LanguageManager.SelectedLanguage ?? "English";

        currentStory = (currentLanguage == "Spanish" && spanishInkJSON != null)
            ? new Story(spanishInkJSON.text)
            : new Story(englishInkJSON.text);

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        nextButton.SetActive(true);
        cameraMain.SetActive(false);
        ContinueStory();
    }

    public void OnNextButtonClicked()
    {
        if (waitingForClick)
            ContinueStory();
    }

    public void OnChoiceSelected(int index)
    {
        currentStory.ChooseChoiceIndex(index);
        HideAllChoices();
        ContinueStory();
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string rawText = currentStory.Continue().Trim();
            string displayText = rawText;

            bool isA = false;

            if (rawText.StartsWith("A:"))
            {
                displayText = rawText.Substring(2).Trim();
                cameraA.SetActive(true);
                cameraB.SetActive(false);
                isA = true;
            }
            else if (rawText.StartsWith("B:"))
            {
                displayText = rawText.Substring(2).Trim();
                cameraB.SetActive(true);
                cameraA.SetActive(false);
                isA = false;
            }

            subtitleText.text = displayText;
            nextButton.SetActive(false);
            waitingForClick = false;

            if (waitForVoiceCoroutine != null)
                StopCoroutine(waitForVoiceCoroutine);

            waitForVoiceCoroutine = StartCoroutine(PlayVoiceThenEnableNext(displayText, isA));
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    void DisplayChoices()
    {
        nextButton.SetActive(false);
        HideAllChoices();

        if (cameraA != null) cameraA.SetActive(false);
        if (cameraB != null) cameraB.SetActive(true);

        for (int i = 0; i < currentStory.currentChoices.Count; i++)
        {
            Choice choice = currentStory.currentChoices[i];
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choice.text;

            int choiceIndex = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        }
    }

    void HideAllChoices()
    {
        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
    }

    void EndDialogue()
    {
        subtitleText.text = "";
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
            Debug.Log("Dialogue canvas deactivated at end.");
        }

        nextButton.SetActive(false);
        HideAllChoices();

        cameraMain.SetActive(true);
        if (cameraA != null) cameraA.SetActive(false);
        if (cameraB != null) cameraB.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        voicePlayer.Stop();
    }

    void LoadVoiceClips()
    {
        AudioClip[] loadedClips = Resources.LoadAll<AudioClip>("Voice");
        foreach (AudioClip clip in loadedClips)
        {
            string nameKey = clip.name.ToLower();
            if (!voiceClips.ContainsKey(nameKey))
            {
                voiceClips.Add(nameKey, clip);
                Debug.Log("Loaded voice clip: " + nameKey);
            }
        }

        Debug.Log("Total voice clips loaded: " + voiceClips.Count);
    }

    IEnumerator PlayVoiceThenEnableNext(string lineText, bool isA)
    {
        if (!isA)
        {
            yield return new WaitForSeconds(0.5f);
            nextButton.SetActive(true);
            waitingForClick = true;
            yield break;
        }

        string key;

        if (LanguageManager.SelectedLanguage == "Spanish")
        {
            // Force this specific line to S4
            if (lineText.Contains("No hay problema") || lineText.Contains("buen rato"))
            {
                key = "s4";
            }
            else if (!aLineIndexMap.ContainsKey(lineText))
            {
                aLineCount++;
                key = "s" + aLineCount;
                aLineIndexMap[lineText] = key;
            }
            else
            {
                key = aLineIndexMap[lineText];
            }
        }
        else
        {
            key = lineText.ToLower()
                .Replace(" ", "")
                .Replace(".", "")
                .Replace("?", "")
                .Replace("!", "")
                .Replace(",", "");
        }

        Debug.Log("Voice key resolved for line: \"" + lineText + "\" => " + key);

        if (voiceClips.ContainsKey(key))
        {
            voicePlayer.Stop();
            voicePlayer.clip = voiceClips[key];
            voicePlayer.Play();
            yield return new WaitWhile(() => voicePlayer.isPlaying);
        }
        else
        {
            Debug.LogWarning("Voice clip not found: " + key);
            yield return new WaitForSeconds(0.5f);
        }

        nextButton.SetActive(true);
        waitingForClick = true;
    }
}
