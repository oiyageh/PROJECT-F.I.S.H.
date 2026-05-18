using UnityEngine;

public class AutoSaveSystem : MonoBehaviour
{
    public Transform player;

    [Header("Auto Save")]
    public float autoSaveInterval = 30f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= autoSaveInterval)
        {
            MainMenu.SaveGame(player);

            timer = 0f;
        }
    }

    // Save when game closes
    void OnApplicationQuit()
    {
        MainMenu.SaveGame(player);
    }
}