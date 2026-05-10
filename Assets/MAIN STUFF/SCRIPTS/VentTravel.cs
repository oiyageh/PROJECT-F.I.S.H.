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

    private bool playerInside;
    private bool showingMessage;

    void Start()
    {
        if (messageUI != null)
            messageUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInside)
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
        for (int i = 0; i < SimpleInventory.Instance.inventory.Count; i++)
        {
            if (SimpleInventory.Instance.inventory[i].itemName == "Screwdriver")
            {
                // remove screwdriver
                SimpleInventory.Instance.inventory.RemoveAt(i);

                ventUnlocked = true;

                ShowMessage(unlockedMessage);

                return;
            }
        }

        ShowMessage(needScrewdriverMessage);
    }

    void EnterVent()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = teleportPoint.position;

        ShowMessage(enterVentMessage);
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

        messageUI.SetActive(true);

        messageText.text = msg;

        yield return new WaitForSeconds(messageDuration);

        messageUI.SetActive(false);

        showingMessage = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}