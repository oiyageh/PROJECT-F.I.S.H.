using UnityEngine;
using TMPro;
using System.Collections;

public class VentTravel : MonoBehaviour
{
    [Header("Vent")]
    public bool ventUnlocked = false;

    [Header("Teleport")]
    public Transform teleportPoint;

    [Header("UI")]
    public GameObject messageUI;
    public TextMeshProUGUI messageText;

    [Header("Messages")]
    [TextArea]
    public string needScrewdriverMessage = "Need a screwdriver";

    [TextArea]
    public string unlockedMessage = "Vent unlocked";

    [TextArea]
    public string enterVentMessage = "Entered vent";

    public float messageDuration = 2f;

    private GameObject currentPlayer;
    private bool showingMessage;

    void Start()
    {
        if (messageUI != null)
            messageUI.SetActive(false);
    }

    void Update()
    {
        if (currentPlayer == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!ventUnlocked)
            {
                TryUnlockVent();
            }
            else
            {
                EnterVent();
            }
        }
    }

    void TryUnlockVent()
    {
        if (SimpleInventory.Instance == null)
            return;

        for (int i = 0; i < SimpleInventory.Instance.inventory.Count; i++)
        {
            if (SimpleInventory.Instance.inventory[i].itemName.ToLower() == "screwdriver")
            {
                // Remove screwdriver
                SimpleInventory.Instance.inventory.RemoveAt(i);

                ventUnlocked = true;

                ShowMessage(unlockedMessage);

                return;
            }

            Debug.Log(SimpleInventory.Instance.inventory[i].itemName);
        }

        ShowMessage(needScrewdriverMessage);
    }

    void EnterVent()
    {
        if (currentPlayer == null || teleportPoint == null)
            return;

        CharacterController controller =
            currentPlayer.GetComponent<CharacterController>();

        // Disable movement briefly
        MonoBehaviour movementScript =
            currentPlayer.GetComponent<ThirdPersonController>();

        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Teleport player
        currentPlayer.transform.position =
            teleportPoint.position + Vector3.up * 0.2f;

        currentPlayer.transform.rotation =
            teleportPoint.rotation;

        // Re-enable movement after short delay
        StartCoroutine(ReEnableMovement(movementScript));

        ShowMessage(enterVentMessage);
    }

    IEnumerator ReEnableMovement(MonoBehaviour movementScript)
    {
        yield return new WaitForSeconds(0.1f);

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }


    void ShowMessage(string msg)
    {
        if (showingMessage)
            return;

        StartCoroutine(MessageRoutine(msg));
    }

    IEnumerator MessageRoutine(string msg)
    {
        showingMessage = true;

        if (messageUI != null)
            messageUI.SetActive(true);

        if (messageText != null)
            messageText.text = msg;

        yield return new WaitForSeconds(messageDuration);

        if (messageUI != null)
            messageUI.SetActive(false);

        showingMessage = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = null;
        }
    }
}