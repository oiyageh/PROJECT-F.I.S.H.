using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait 1 frame for scene objects to load
        yield return null;

        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No player found");
            yield break;
        }

        // Get spawn point name from GameManager
        string spawnName = GameManager.Instance.spawnPointName;

        if (string.IsNullOrEmpty(spawnName))
        {
            yield break;
        }

        // Find spawn point object
        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point not found: " + spawnName);
            yield break;
        }

        // Disable CharacterController before moving
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        // Move player
        player.transform.position = spawnPoint.transform.position;

        // Re-enable controller
        if (cc != null)
            cc.enabled = true;
    }
}