using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)]
        public string text;
        public Sprite characterSprite;
    }

    [Header("Dialogue ID")]
    public string dialogueID = "intro_dialogue";
    public bool playOnlyOnce = true;

    [Header("Save System")]
    public bool autoSaveAfterDialogue = false;
    public string dialogueCheckpointName = "";

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image characterImage;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Player / Gameplay")]
    public MonoBehaviour playerController;
    public GameObject pauseObjects;

    [Header("Dialogue")]
    public DialogueLine[] lines;

    private int currentLine;
    private bool isTyping;
    private bool dialogueActive;

    void Start()
    {
        dialoguePanel.SetActive(false);

        // Auto-disable if already seen
        if (playOnlyOnce &&
            GameManager.Instance != null &&
            GameManager.Instance.HasSeenDialogue(dialogueID))
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[currentLine].text;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        // Prevent replay
        if (playOnlyOnce &&
            GameManager.Instance != null &&
            GameManager.Instance.HasSeenDialogue(dialogueID))
        {
            return;
        }

        if (lines.Length == 0) return;

        dialogueActive = true;
        currentLine = 0;

        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerController != null)
            playerController.enabled = false;

        if (pauseObjects != null)
            pauseObjects.SetActive(false);

        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());

        if (characterImage != null && lines[currentLine].characterSprite != null)
            characterImage.sprite = lines[currentLine].characterSprite;
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in lines[currentLine].text)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);

        Time.timeScale = 1f;

        if (playerController != null)
            playerController.enabled = true;

        if (pauseObjects != null)
            pauseObjects.SetActive(true);

        // MARK DIALOGUE AS SEEN
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkDialogueSeen(dialogueID);
        }

        // SAVE
        if (autoSaveAfterDialogue && GameManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(dialogueCheckpointName))
            {
                GameManager.Instance.spawnPointName = dialogueCheckpointName;
            }

            GameManager.Instance.SaveGame(
                SceneManager.GetActiveScene().name
            );

            Debug.Log("Game auto-saved after dialogue.");
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}