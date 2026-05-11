using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleWorldInteraction : MonoBehaviour
{
    [Header("Player")]
    public Transform playerCamera;
    public MonoBehaviour playerController;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public GameObject promptUI;
    public GameObject puzzleUI;

    [Header("Camera Zoom")]
    public Transform cameraViewPoint;
    public float zoomSpeed = 4f;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeSpeed = 2f;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private Transform player;

    private bool inPuzzle = false;
    private bool isTransitioning = false;

    IEnumerator Start()
    {
        puzzleUI.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        // WAIT FOR PLAYER TO EXIST
        yield return StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            GameObject foundPlayer =
                GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;

                // Auto assign camera if missing
                if (playerCamera == null)
                    playerCamera = Camera.main.transform;
            }

            yield return null;
        }
    }

    void Update()
    {
        // Safety check
        if (player == null || playerCamera == null)
            return;

        if (isTransitioning)
            return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        bool canInteract = distance <= interactDistance;

        if (promptUI != null)
            promptUI.SetActive(canInteract && !inPuzzle);

        if (canInteract && Input.GetKeyDown(interactKey))
        {
            if (!inPuzzle)
                StartCoroutine(OpenPuzzle());
            else
                StartCoroutine(ClosePuzzle());
        }
    }

    IEnumerator OpenPuzzle()
    {
        isTransitioning = true;
        inPuzzle = true;

        originalCamPos = playerCamera.position;
        originalCamRot = playerCamera.rotation;

        if (playerController != null)
            playerController.enabled = false;

        // Fade out
        yield return StartCoroutine(Fade(1));

        // Move camera
        yield return StartCoroutine(MoveCamera(
            cameraViewPoint.position,
            cameraViewPoint.rotation));

        // Fade in
        yield return StartCoroutine(Fade(0));

        puzzleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        isTransitioning = false;
    }

    IEnumerator ClosePuzzle()
    {
        isTransitioning = true;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        puzzleUI.SetActive(false);

        // Fade out
        yield return StartCoroutine(Fade(1));

        // Restore camera
        yield return StartCoroutine(MoveCamera(
            originalCamPos,
            originalCamRot));

        // Fade in
        yield return StartCoroutine(Fade(0));

        if (playerController != null)
            playerController.enabled = true;

        inPuzzle = false;
        isTransitioning = false;
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0;

        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * zoomSpeed;

            playerCamera.position =
                Vector3.Lerp(startPos, targetPos, t);

            playerCamera.rotation =
                Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);

            fadeImage.color = c;

            yield return null;
        }
    }
}