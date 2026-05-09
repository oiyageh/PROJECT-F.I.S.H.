using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Potion";
    public int uses = 3;
    public Sprite itemIcon;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SimpleInventory.Instance.AddItem(itemName, uses, itemIcon);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}