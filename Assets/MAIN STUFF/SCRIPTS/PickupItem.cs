using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName = "Potion";
    public int uses = 3;

    bool playerNearby;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            SimpleInventory.Instance.AddItem(itemName, uses);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}