using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn")]
    public string spawnPointName;

    private HashSet<string> unlockedDoors = new HashSet<string>();

    private void Start()
    {
        Debug.Log("GameManager alive");
    }

    public Dictionary<string, List<string>> savedPuzzleParts =
        new Dictionary<string, List<string>>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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