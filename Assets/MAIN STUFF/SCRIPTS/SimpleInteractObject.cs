using UnityEngine;
using TMPro;

public class SimpleInteractObject : MonoBehaviour
{
    [Header("Item")]
    public string itemName = "Screwdriver";
    public int uses = 1;
    public Sprite icon;

    [Header("UI")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;

    [TextArea]
    public string pickupMessage = "Press E to pick up item";

    private bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        SimpleInventory.Instance.AddItem(itemName, uses, icon);

        if (promptUI != null)
            promptUI.SetActive(false);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (promptUI != null)
        {
            promptUI.SetActive(true);

            if (promptText != null)
            {
                promptText.text = itemName + "\n" + pickupMessage;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptUI != null)
            promptUI.SetActive(false);
    }
}