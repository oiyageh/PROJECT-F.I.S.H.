using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleInventory : MonoBehaviour
{
    public static SimpleInventory Instance;

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int usesRemaining;
        public Sprite icon;

        public InventoryItem(string name, int uses, Sprite itemIcon)
        {
            itemName = name;
            usesRemaining = uses;
            icon = itemIcon;
        }
    }

    [Header("Inventory")]
    public List<InventoryItem> inventory = new List<InventoryItem>();

    [Header("Hotbar")]
    public int hotbarSize = 5;
    public int selectedSlot = 0;

    [Header("UI")]
    public GameObject slotPrefab;
    public Transform slotParent;

    private GameObject[] slots;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        // Select slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedSlot = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedSlot = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedSlot = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedSlot = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5))
            selectedSlot = 4;

        // Use selected item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSelectedItem();
        }

        UpdateUI();
    }

    void CreateSlots()
    {
        slots = new GameObject[hotbarSize];

        for (int i = 0; i < hotbarSize; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent);

            slots[i] = newSlot;
        }
    }

    void UpdateUI()
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            TMP_Text text = slots[i].GetComponentInChildren<TMP_Text>();

            Transform iconTransform = slots[i].transform.Find("Icon");

            if (iconTransform == null)
            {
                Debug.LogWarning("Icon object missing from slot prefab.");
                continue;
            }

            Image iconImage = iconTransform.GetComponent<Image>();

            Image background = slots[i].GetComponent<Image>();

            // Highlight selected slot
            background.color = (i == selectedSlot) ? Color.yellow : Color.white;

            // Show item
            if (i < inventory.Count)
            {
                InventoryItem item = inventory[i];

                text.text = item.usesRemaining.ToString();

                iconImage.sprite = item.icon;
                iconImage.enabled = true;
            }
            else
            {
                text.text = "";

                iconImage.enabled = false;
            }
        }
    }

    public void AddItem(string itemName, int uses, Sprite icon)
    {
        inventory.Add(new InventoryItem(itemName, uses, icon));

        Debug.Log(itemName + " added.");
    }

    public void UseSelectedItem()
    {
        if (inventory.Count == 0)
            return;

        if (selectedSlot >= inventory.Count)
            return;

        InventoryItem item = inventory[selectedSlot];

        item.usesRemaining--;

        Debug.Log(item.itemName + " used.");

        if (item.usesRemaining <= 0)
        {
            Debug.Log(item.itemName + " removed.");

            inventory.RemoveAt(selectedSlot);

            if (selectedSlot >= inventory.Count)
            {
                selectedSlot = Mathf.Max(0, inventory.Count - 1);
            }
        }
    }
}