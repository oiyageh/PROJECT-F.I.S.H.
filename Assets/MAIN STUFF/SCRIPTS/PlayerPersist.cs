using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPersist : MonoBehaviour
{
    private static PlayerPersist instance;

    private CharacterController controller;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            controller = GetComponent<CharacterController>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.spawnName == GameManager.Instance.spawnPointName)
            {
                // Disable controller before teleport
                if (controller != null)
                    controller.enabled = false;

                transform.position = point.transform.position;
                transform.rotation = point.transform.rotation;

                // Re-enable controller
                if (controller != null)
                    controller.enabled = true;

                return;
            }
        }

        Debug.LogWarning("Spawn point not found: " + GameManager.Instance.spawnPointName);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}