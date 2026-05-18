using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn")]
    public string spawnPointName;

    private HashSet<string> unlockedDoors = new HashSet<string>();
    private HashSet<string> seenDialogues = new HashSet<string>();

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

    // =========================
    // DOOR SYSTEM
    // =========================
    public void UnlockDoor(string doorID)
    {
        unlockedDoors.Add(doorID);
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return unlockedDoors.Contains(doorID);
    }

    // =========================
    // SIMPLE SAVE SYSTEM (BASIC CHECKPOINT)
    // =========================
    public void SaveGame(string sceneName)
    {
        PlayerPrefs.SetString("Scene", sceneName);
        PlayerPrefs.SetString("SpawnPoint", spawnPointName);

        PlayerPrefs.Save();

        Debug.Log($"Game Saved → Scene: {sceneName}, Spawn: {spawnPointName}");
    }

    public void LoadGame(out string sceneName, out string spawnPoint)
    {
        sceneName = PlayerPrefs.GetString("Scene", SceneManager.GetActiveScene().name);
        spawnPoint = PlayerPrefs.GetString("SpawnPoint", "");
    }

    // =========================
    // DIALOGUE SYSTEM
    // =========================
    public void MarkDialogueSeen(string dialogueID)
    {
        seenDialogues.Add(dialogueID);
    }

    public bool HasSeenDialogue(string dialogueID)
    {
        return seenDialogues.Contains(dialogueID);
    }
}