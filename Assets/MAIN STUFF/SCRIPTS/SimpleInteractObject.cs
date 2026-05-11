using UnityEngine;
using TMPro;

public class SimpleInteractObject : MonoBehaviour
{
    [Header("Type")]
    public bool isVent;
    public bool givesItem;

    [Header("Vent Settings")]
    public bool alreadyOpen;
    public VentSystem ventSystem;

    [Header("Item")]
    public string itemName = "Screwdriver";
    public int uses = 1;
    public Sprite icon;


    private bool playerInRange;
    private GameObject player;
    private bool opened;

    void Start()
    {
        
    }

    void Update()
    {
        if (!playerInRange) return;

       

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        // PICKUP ITEM
        if (givesItem)
        {
            SimpleInventory.Instance.AddItem(itemName, uses, icon);
            Destroy(gameObject);
            return;
        }

        if (isVent)
        {
            // If vent is already opened OR pre-opened
            if (alreadyOpen || opened)
            {
                if (ventSystem != null)
                    ventSystem.UseVent(player);

                return;
            }

            // Try use screwdriver
            for (int i = 0; i < SimpleInventory.Instance.inventory.Count; i++)
            {
                var item = SimpleInventory.Instance.inventory[i];

                if (item.itemName.ToLower() == "screwdriver")
                {
                    item.usesRemaining--;

                    if (item.usesRemaining <= 0)
                        SimpleInventory.Instance.inventory.RemoveAt(i);

                    opened = true;

                    Debug.Log("Vent permanently opened");

                    // OPTIONAL: instantly allow travel after opening
                    if (ventSystem != null)
                        ventSystem.UseVent(player);

                    return;
                }
            }

            Debug.Log("Need screwdriver");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;

            
        }
    }
}