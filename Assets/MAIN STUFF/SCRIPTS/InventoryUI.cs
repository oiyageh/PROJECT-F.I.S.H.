using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;

    private GameObject[] slots;

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        if (SimpleInventory.Instance == null)
            return;

        if (slots == null)
            return;

        UpdateUI();
    }

    void CreateSlots()
    {
        int size = 5;

        slots = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);

            slots[i] = newSlot;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Skip broken slots
            if (slots[i] == null)
                continue;

            TMP_Text text = slots[i].GetComponentInChildren<TMP_Text>();
            Image image = slots[i].GetComponent<Image>();

            // Skip if components missing
            if (text == null || image == null)
            {
                Debug.LogWarning("Slot prefab missing TMP_Text or Image component.");
                continue;
            }

            // Highlight selected slot
            if (i == SimpleInventory.Instance.selectedSlot)
            {
                image.color = Color.yellow;
            }
            else
            {
                image.color = Color.white;
            }

            // Show inventory items
            if (i < SimpleInventory.Instance.inventory.Count)
            {
                var item = SimpleInventory.Instance.inventory[i];

                text.text = item.itemName + "\nUses: " + item.usesRemaining;
            }
            else
            {
                text.text = "";
            }
        }
    }
}