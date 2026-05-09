using UnityEngine;

public class VentTravel : MonoBehaviour
{
    [Header("Vent")]
    public bool ventUnlocked = false;

    [Header("Teleport")]
    public Transform teleportPoint;

    private bool playerInside;

    void Update()
    {
        if (!playerInside)
            return;

        // PRESS E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // UNLOCK VENT
            if (!ventUnlocked)
            {
                TryUnlockVent();
            }
            else
            {
                EnterVent();
            }
        }
    }

    void TryUnlockVent()
    {
        for (int i = 0; i < SimpleInventory.Instance.inventory.Count; i++)
        {
            if (SimpleInventory.Instance.inventory[i].itemName == "Screwdriver")
            {
                // remove screwdriver
                SimpleInventory.Instance.inventory.RemoveAt(i);

                ventUnlocked = true;

                Debug.Log("Vent unlocked!");

                return;
            }
        }

        Debug.Log("Need screwdriver");
    }

    void EnterVent()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = teleportPoint.position;

        Debug.Log("Entered vent");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}