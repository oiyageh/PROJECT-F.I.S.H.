using UnityEngine;

public class SimpleInteractObject : MonoBehaviour
{
    [Header("Type")]
    public bool isVent;
    public bool givesItem;

    [Header("Item (if drawer)")]
    public string itemName = "Screwdriver";
    public int uses = 1;
    public Sprite icon;

    private bool opened;

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        // DRAWER / ITEM
        if (givesItem)
        {
            SimpleInventory.Instance.AddItem(itemName, uses, icon);
            Debug.Log("Picked up " + itemName);
            return;
        }

        // VENT
        if (isVent)
        {
            bool hasTool = false;

            foreach (var item in SimpleInventory.Instance.inventory)
            {
                if (item.itemName == "Screwdriver")
                    hasTool = true;
            }

            if (!hasTool)
            {
                Debug.Log("Need screwdriver");
                return;
            }

            Debug.Log("Vent opened!");
            opened = true;
        }
    }
}