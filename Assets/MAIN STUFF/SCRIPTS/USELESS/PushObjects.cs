using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public float force = 3f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Movable")) return;

        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null) return;

        Vector3 dir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}