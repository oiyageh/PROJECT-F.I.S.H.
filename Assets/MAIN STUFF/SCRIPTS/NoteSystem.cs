using UnityEngine;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";

    [Header("Note Info")]
    public string noteTitle = "Note";

    [TextArea(5, 15)]
    public string noteText;

    [Header("UI")]
    public GameObject notePanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Controls")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInside;
    private bool readingNote;
    private bool collected;

    void Start()
    {
        if (notePanel != null)
        {
            notePanel.SetActive(false);
        }
    }

    void Update()
    {
        // OPEN NOTE
        if (playerInside && !readingNote && !collected && Input.GetKeyDown(interactKey))
        {
            OpenNote();
        }

        // CLOSE NOTE
        else if (readingNote && Input.GetKeyDown(interactKey))
        {
            CloseNote();
        }
    }

    void OpenNote()
    {
        // Safety checks
        if (notePanel == null || titleText == null || bodyText == null)
        {
            Debug.LogError("NOTE UI REFERENCES ARE MISSING!");
            return;
        }

        readingNote = true;
        collected = true;

        notePanel.SetActive(true);

        titleText.text = noteTitle;
        bodyText.text = noteText;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide note object
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        if (mesh != null)
        {
            mesh.enabled = false;
        }

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }
    }

    void CloseNote()
    {
        readingNote = false;

        notePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
        }
    }
}