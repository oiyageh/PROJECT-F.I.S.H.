using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{
    [Header("Item")]
    public string itemName = "Missing Heart";
    public int uses = 1;
    public Sprite itemIcon;

    [Header("UI")]
    public TextMeshProUGUI promptText;

    private bool inRange;

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            SimpleInventory.Instance.AddItem(
                itemName,
                uses,
                itemIcon
            );

            Debug.Log(itemName + " picked up.");

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = "Press E to pick up " + itemName;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = false;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}