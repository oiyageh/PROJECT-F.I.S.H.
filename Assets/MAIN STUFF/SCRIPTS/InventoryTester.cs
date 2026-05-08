using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    void Update()
    {
        // Left click uses currently selected hotbar item
        if (Input.GetMouseButtonDown(0))
        {
            SimpleInventory.Instance.UseSelectedItem();
        }
    }
}