using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSystem dialogue;

    private bool hasPlayed;
    [Header("Save Point")]
    public string checkpointName;
    public bool saveAfterDialogue = true;

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
            return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            dialogue.autoSaveAfterDialogue = saveAfterDialogue;
            dialogue.dialogueCheckpointName = checkpointName;

            dialogue.StartDialogue();

            gameObject.SetActive(false);
        }
    }
}