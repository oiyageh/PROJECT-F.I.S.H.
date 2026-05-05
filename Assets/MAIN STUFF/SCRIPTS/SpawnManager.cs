using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; // wait for scene to load

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        string spawnName = GameManager.Instance.spawnPointName;
        if (string.IsNullOrEmpty(spawnName)) yield break;

        GameObject spawnPoint = GameObject.Find(spawnName);
        if (spawnPoint == null) yield break;

        // Handle CharacterController properly
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 pos = spawnPoint.transform.position + Vector3.up * 2f;
        player.transform.position = pos;

        // Snap to ground
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 5f))
        {
            player.transform.position = hit.point;
        }

        if (cc != null) cc.enabled = true;
    }
}