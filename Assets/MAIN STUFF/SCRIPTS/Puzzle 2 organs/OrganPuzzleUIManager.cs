using UnityEngine;
using TMPro;

public class OrganPuzzleUIManager : MonoBehaviour
{
    public OrganUISlot[] slots;

    public string unlockCode = "4721";
    public TextMeshProUGUI messageText;

    public KeyCode exitKey = KeyCode.E;
    public GameObject puzzleUI;

    void Update()
    {
        if (puzzleUI.activeSelf && Input.GetKeyDown(exitKey))
        {
            ClosePuzzle();
        }
    }

    public void CheckPuzzle()
    {
        bool allFilled = true;
        bool allCorrect = true;

        foreach (var slot in slots)
        {
            if (!slot.IsFilled())
                allFilled = false;

            if (!slot.IsCorrect())
                allCorrect = false;
        }

        if (!allFilled)
        {
            messageText.text = "Something is missing...";
            return;
        }

        if (!allCorrect)
        {
            messageText.text = "Something is wrong...";
            return;
        }

        messageText.text = "Code: " + unlockCode;
    }

    public void ClosePuzzle()
    {
        puzzleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}