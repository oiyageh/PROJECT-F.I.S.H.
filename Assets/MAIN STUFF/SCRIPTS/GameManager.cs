using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn")]
    public string spawnPointName;

    // Door persistence
    private HashSet<string> unlockedDoors = new HashSet<string>();

    // Puzzle persistence
    public Dictionary<string, List<string>> savedPuzzleParts =
        new Dictionary<string, List<string>>();

    private void Awake()
    {
        // Prevent duplicate GameManagers
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Persist through scenes
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager alive");
    }

    // =========================
    // DOOR SAVE SYSTEM
    // =========================

    public void UnlockDoor(string doorID)
    {
        if (!unlockedDoors.Contains(doorID))
        {
            unlockedDoors.Add(doorID);
        }
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return unlockedDoors.Contains(doorID);
    }
}