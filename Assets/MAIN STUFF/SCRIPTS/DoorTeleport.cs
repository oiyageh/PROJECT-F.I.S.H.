using System.Collections;
using UnityEngine;


public class DoorTeleport : MonoBehaviour
{
    [Header("Teleport")]
    [SerializeField] private Transform destination;
    [SerializeField] private Collider2D newRoomCollider;

    [Header("Camera")]
   // [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private float lensZoom = 6.14f;
    [SerializeField] private GameObject[] disableObjects;

    [Header("Transition")]
    [SerializeField] private Animator screenWipeAnimator;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TeleportRoutine(collision.gameObject));
        }
    }

    private IEnumerator TeleportRoutine(GameObject player)
    {
        isTransitioning = true;

        // Disable movement
        DisablePlayer(player);

        // Play wipe out animation
        screenWipeAnimator.SetTrigger("Out");

        // Wait for wipe animation to finish
        yield return new WaitForSeconds(2f);

        // Switch camera confiner
       // SwitchConfiner(newRoomCollider);

        // Teleport player
        player.transform.position = destination.position;





        // safeguard wait
        yield return new WaitForSeconds(.5f);

        DisableGameObjects();

       // SwitchZoom(lensZoom); //switch lens zoom of cinemachine camera

        // Play wipe in animation
        screenWipeAnimator.SetTrigger("In");

        // Wait for wipe in to finish
        yield return new WaitForSeconds(1f);

        // Re-enable movement
        EnablePlayer(player);

        isTransitioning = false;
    }
    /*

    public void SwitchConfiner(Collider2D nextCollider)
    {
        var confiner = playerCam.GetComponent<CinemachineConfiner2D>();


        if (confiner != null)
        {
            confiner.BoundingShape2D = nextCollider;
            confiner.InvalidateBoundingShapeCache();
        }
    }*/
    /*
    public void SwitchZoom(float zoom)
    {
        var cam = playerCam.GetComponent<CinemachineCamera>();


        cam.Lens.OrthographicSize = zoom;
    }
    */
    private void DisablePlayer(GameObject player)
    {
        var movement = player.GetComponent<ThirdPersonController>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void EnablePlayer(GameObject player)
    {
        var movement = player.GetComponent<ThirdPersonController>();

        if (movement != null)
        {
            movement.enabled = true;
        }
    }

    private void DisableGameObjects()
    {
        foreach(var gameObj in disableObjects)
        {
            gameObj.SetActive(false);
        }
    }
}