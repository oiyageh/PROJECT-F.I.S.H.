using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Scene Transition")]
    public string sceneToLoad;
    public string spawnPointName;

    [Header("Lock Settings")]
    public bool isBlocked = false;
    public string requiredItem;

    [Header("Persistence")]
    public string doorID;

    private bool playerInRange;

    void Start()
    {
        // If already unlocked, permanently unlock it
        if (!string.IsNullOrEmpty(doorID) && GameManager.Instance.IsDoorUnlocked(doorID))
        {
            isBlocked = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        // If blocked, check item
        if (isBlocked)
        {
            if (!SimpleInventory.Instance.HasItem(requiredItem))
            {
                Debug.Log("You need " + requiredItem);
                return;
            }

            // consume item (optional)
            SimpleInventory.Instance.UseItemByName(requiredItem);

            // unlock permanently
            isBlocked = false;

            if (!string.IsNullOrEmpty(doorID))
            {
                GameManager.Instance.UnlockDoor(doorID);
            }
        }

        GameManager.Instance.spawnPointName = spawnPointName;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}