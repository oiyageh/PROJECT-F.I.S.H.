using UnityEngine;

public class SimpleInteractObject : MonoBehaviour
{
    [Header("Type")]
    public bool isVent;
    public bool givesItem;

    [Header("Item")]
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
        // GIVE ITEM
        if (givesItem)
        {
            SimpleInventory.Instance.AddItem(itemName, uses, icon);

            Debug.Log("Picked up " + itemName);

            Destroy(gameObject);

            return;
        }

        // OPEN VENT
        if (isVent)
        {
            if (opened) return;

            for (int i = 0; i < SimpleInventory.Instance.inventory.Count; i++)
            {
                if (SimpleInventory.Instance.inventory[i].itemName == "Screwdriver")
                {
                    // REMOVE screwdriver after use
                    SimpleInventory.Instance.inventory[i].usesRemaining--;

                    if (SimpleInventory.Instance.inventory[i].usesRemaining <= 0)
                    {
                        SimpleInventory.Instance.inventory.RemoveAt(i);
                    }
                }
            }

            Debug.Log("Need screwdriver");
        }
    }
}