using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn")]
    public string spawnPointName;

    private HashSet<string> unlockedDoors = new HashSet<string>();

    private void Awake()
    {
        // If another GameManager already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set singleton
        Instance = this;

        // Persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockDoor(string doorID)
    {
        unlockedDoors.Add(doorID);
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return unlockedDoors.Contains(doorID);
    }
}