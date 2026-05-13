using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSceneDoor : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterManager.nextSpawn = spawnPointName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}