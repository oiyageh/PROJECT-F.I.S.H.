using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform inventoryPanel;

    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    private GameObject[] slots;

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        UpdateUI();
    }

    void CreateSlots()
    {
        int size = SimpleInventory.Instance.hotbarSize;

        slots = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel);

            slots[i] = slot;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Image bg = slots[i].GetComponent<Image>();
            TMP_Text text = slots[i].GetComponentInChildren<TMP_Text>();

            if (i == SimpleInventory.Instance.selectedSlot)
            {
                bg.color = selectedColor;
            }
            else
            {
                bg.color = normalColor;
            }

            if (i < SimpleInventory.Instance.inventory.Count)
            {
                var item = SimpleInventory.Instance.inventory[i];

                text.text = item.itemName + "\n" + item.usesRemaining;
            }
            else
            {
                text.text = "";
            }
        }
    }
}