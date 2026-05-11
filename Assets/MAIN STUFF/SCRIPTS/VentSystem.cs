using UnityEngine;
using System.Collections;

public class VentSystem : MonoBehaviour
{
    [Header("Linked Vent")]
    public VentSystem targetVent;

    public void UseVent(GameObject player)
    {
        if (player == null || targetVent == null)
        {
            Debug.LogWarning("Vent link missing!");
            return;
        }

        StartCoroutine(Teleport(player));
    }

    IEnumerator Teleport(GameObject player)
    {
        Transform target = targetVent.transform;

        CharacterController cc = player.GetComponent<CharacterController>();
        ThirdPersonController move = player.GetComponent<ThirdPersonController>();

        if (cc) cc.enabled = false;
        if (move) move.enabled = false;

        yield return null;

        player.transform.position = target.position + Vector3.up * 0.2f;
        player.transform.rotation = target.rotation;

        yield return null;

        if (cc) cc.enabled = true;
        if (move) move.enabled = true;

        Debug.Log("Vent travel complete");
    }
}