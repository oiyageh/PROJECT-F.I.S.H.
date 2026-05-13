using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public static string nextSpawn;

    public GameObject monster;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MonsterSpawnPoint[] spawns =
            FindObjectsOfType<MonsterSpawnPoint>();

        foreach (MonsterSpawnPoint spawn in spawns)
        {
            if (spawn.spawnName == nextSpawn)
            {
                monster.transform.position =
                    spawn.transform.position;

                break;
            }
        }
    }
}