using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Screwdriver";
    public int uses = 1;
    public Sprite itemIcon;

    private bool inRange;

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            SimpleInventory.Instance.AddItem(itemName, uses, itemIcon);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inRange = false;
    }
}