using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CodeLockedDoor : MonoBehaviour
{
    [Header("Scene Transition")]
    public string sceneToLoad;
    public string spawnPointName;

    [Header("Requirements")]
    public bool requiresItem = true;
    public string requiredItem;

    public bool requiresCode = true;
    public string requiredCode = "1234";

    [Header("Persistence")]
    public string doorID;

    [Header("UI")]
    public GameObject lockedMessageUI;
    public TextMeshProUGUI lockedMessageText;

    [Header("Code UI")]
    public GameObject codePanelUI;
    public TMP_InputField codeInputField;

    [Header("Messages")]
    [TextArea]
    public string lockedMessage = "The door is locked.";

    [TextArea]
    public string wrongCodeMessage = "Wrong code.";

    private bool playerInRange;
    private bool unlocked = false;

    void Start()
    {
        // Hide UI at start
        if (lockedMessageUI != null)
            lockedMessageUI.SetActive(false);

        if (codePanelUI != null)
            codePanelUI.SetActive(false);

        // Check saved unlock
        if (!string.IsNullOrEmpty(doorID) && GameManager.Instance.IsDoorUnlocked(doorID))
        {
            unlocked = true;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        // Already unlocked
        if (unlocked)
        {
            OpenDoor();
            return;
        }

        // Check item
        if (requiresItem)
        {
            if (!SimpleInventory.Instance.HasItem(requiredItem))
            {
                ShowMessage(lockedMessage + "\nNeed: " + requiredItem);
                return;
            }
        }

        // Check code
        if (requiresCode)
        {
            if (codePanelUI != null)
            {
                codePanelUI.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            return;
        }

        UnlockDoor();
    }

    public void SubmitCode()
    {
        if (codeInputField == null)
            return;

        // Correct code
        if (codeInputField.text == requiredCode)
        {
            UnlockDoor();
        }
        else
        {
            ShowMessage(wrongCodeMessage);
        }
    }

    void UnlockDoor()
    {
        unlocked = true;

        // Consume item
        if (requiresItem)
        {
            SimpleInventory.Instance.UseItemByName(requiredItem);
        }

        // Save unlock
        if (!string.IsNullOrEmpty(doorID))
        {
            GameManager.Instance.UnlockDoor(doorID);
        }

        // Hide UI
        if (codePanelUI != null)
            codePanelUI.SetActive(false);

        if (lockedMessageUI != null)
            lockedMessageUI.SetActive(false);

        OpenDoor();
    }

    void OpenDoor()
    {
        GameManager.Instance.spawnPointName = spawnPointName;
        SceneManager.LoadScene(sceneToLoad);
    }

    void ShowMessage(string message)
    {
        if (lockedMessageUI != null)
            lockedMessageUI.SetActive(true);

        if (lockedMessageText != null)
            lockedMessageText.text = message;
    }

    public void CloseCodePanel()
    {
        if (codePanelUI != null)
            codePanelUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (lockedMessageUI != null)
                lockedMessageUI.SetActive(false);

            if (codePanelUI != null)
                codePanelUI.SetActive(false);
        }
    }
}