using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public string playerTag = "Player";

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask noteLayer;

    [Header("UI")]
    public GameObject notePanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Controls")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode closeKey = KeyCode.E;

    [System.Serializable]
    public class Note
    {
        public string noteID;

        public string noteTitle;

        [TextArea(5, 15)]
        public string noteText;

        public GameObject noteObject;
    }

    [Header("Notes")]
    public List<Note> notes = new List<Note>();

    private bool readingNote = false;

    private List<string> collectedNotes = new List<string>();

    void Start()
    {
        notePanel.SetActive(false);

        FindPlayer();
    }

    void Update()
    {
        // Keep searching if player is missing
        if (player == null)
        {
            FindPlayer();
            return;
        }

        // Close note
        if (readingNote)
        {
            if (Input.GetKeyDown(closeKey))
            {
                CloseNote();
            }

            return;
        }

        // Pick up note
        if (Input.GetKeyDown(interactKey))
        {
            TryPickupNote();
        }
    }

    void FindPlayer()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);

        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }

    void TryPickupNote()
    {
        Ray ray = new Ray(player.position, player.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, noteLayer))
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (hit.collider.gameObject == notes[i].noteObject)
                {
                    OpenNote(notes[i]);

                    if (!collectedNotes.Contains(notes[i].noteID))
                    {
                        collectedNotes.Add(notes[i].noteID);
                    }

                    notes[i].noteObject.SetActive(false);

                    break;
                }
            }
        }
    }

    void OpenNote(Note note)
    {
        readingNote = true;

        notePanel.SetActive(true);

        titleText.text = note.noteTitle;
        bodyText.text = note.noteText;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseNote()
    {
        readingNote = false;

        notePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool HasCollected(string noteID)
    {
        return collectedNotes.Contains(noteID);
    }

    void OnDrawGizmos()
    {
        if (player == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(player.position, player.forward * interactDistance);
    }
}