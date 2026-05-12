using UnityEngine;
using TMPro;

public class OrganPuzzleUIManager : MonoBehaviour
{
    [Header("Puzzle")]
    public OrganUISlot[] slots;

    [Header("Reward")]
    public string unlockCode = "4721";

    [Header("Missing Piece")]
    public bool requiresMissingPiece = true;

    [Tooltip("Exact inventory item name")]
    public string requiredPieceName = "Missing Heart";

    [Header("UI")]
    public TextMeshProUGUI messageText;

    [Header("Puzzle UI")]
    public KeyCode exitKey = KeyCode.E;
    public GameObject puzzleUI;

    private bool puzzleCompleted = false;

    void Update()
    {
        if (puzzleUI.activeSelf && Input.GetKeyDown(exitKey))
        {
            ClosePuzzle();
        }
    }

    public void CheckPuzzle()
    {
        // Prevent repeating completion
        if (puzzleCompleted)
            return;

        bool allFilled = true;
        bool allCorrect = true;

        foreach (var slot in slots)
        {
            if (!slot.IsFilled())
                allFilled = false;

            if (!slot.IsCorrect())
                allCorrect = false;
        }

        // Missing organs in slots
        if (!allFilled)
        {
            messageText.text = "Something is missing...";
            return;
        }

        // Wrong organs
        if (!allCorrect)
        {
            messageText.text = "Something is wrong...";
            return;
        }

        // Check inventory for missing puzzle piece
        if (requiresMissingPiece)
        {
            if (SimpleInventory.Instance == null)
            {
                messageText.text = "Inventory missing.";
                return;
            }

            bool hasPiece =
                SimpleInventory.Instance.HasItem(requiredPieceName);

            if (!hasPiece)
            {
                messageText.text =
                    "You still need the " + requiredPieceName + ".";
                return;
            }

            // REMOVE ITEM FROM INVENTORY
            SimpleInventory.Instance.UseItemByName(requiredPieceName);

            Debug.Log(requiredPieceName + " consumed.");
        }

        puzzleCompleted = true;

        messageText.text = "Code: " + unlockCode;

        Debug.Log("Puzzle completed.");
    }

    public void ClosePuzzle()
    {
        puzzleUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}