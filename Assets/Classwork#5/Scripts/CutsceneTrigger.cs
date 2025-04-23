using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public GameObject player;
    public MonoBehaviour playerMovementScript;
    public DialogueManager dialogueManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (playerMovementScript != null)
                playerMovementScript.enabled = false;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(true);

            dialogueManager.StartDialogue();
        }
    }
}
