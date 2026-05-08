using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    public static SimpleInventory Instance;

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int usesRemaining;

        public InventoryItem(string name, int uses)
        {
            itemName = name;
            usesRemaining = uses;
        }
    }

    public List<InventoryItem> inventory = new List<InventoryItem>();

    [Header("Hotbar")]
    public int selectedSlot = 0;
    public int hotbarSize = 5;

    private void Awake()
    {
        Instance = this;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSelectedItem();
        }
    }

    public void AddItem(string itemName, int uses)
    {
        inventory.Add(new InventoryItem(itemName, uses));

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