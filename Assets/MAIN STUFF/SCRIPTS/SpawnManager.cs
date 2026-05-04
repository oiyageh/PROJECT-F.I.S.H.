using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; // wait 1 frame so scene fully loads

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        string spawnName = GameManager.Instance.spawnPointName;
        if (string.IsNullOrEmpty(spawnName)) yield break;

        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;

            // Reset physics (VERY important if using Rigidbody)
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("Spawn point not found: " + spawnName);
        }
    }
}