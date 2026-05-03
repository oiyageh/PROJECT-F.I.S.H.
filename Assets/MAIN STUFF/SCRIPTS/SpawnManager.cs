using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        string spawnName = GameManager.Instance.spawnPointName;

        if (string.IsNullOrEmpty(spawnName)) return;

        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }
}