using System.Collections;
using System.Collections.Generic;
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

    [Header("Persistence")]
    public string puzzleID;

    [Tooltip("Parts that should be restored automatically")]
    public List<GameObject> requiredParts =
        new List<GameObject>();

    private List<string> insertedParts =
        new List<string>();

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private Transform player;

    private bool inPuzzle = false;
    private bool isTransitioning = false;

    private bool isFixed;

    public ThirdPersonCamera thirdPersonCam;

    IEnumerator Start()
    {
        puzzleUI.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);

        yield return StartCoroutine(FindPlayer());

        LoadPuzzle();
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

                if (playerCamera == null &&
                    Camera.main != null)
                {
                    playerCamera =
                        Camera.main.transform;
                }
            }

            yield return null;
        }
    }

    void Update()
    {
        // Re-find player after scene reload
        if (player == null)
        {
            GameObject foundPlayer =
                GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        // Re-find camera
        if (playerCamera == null &&
            Camera.main != null)
        {
            playerCamera =
                Camera.main.transform;
        }

        if (player == null || playerCamera == null)
            return;

        if (isTransitioning)
            return;

        float distance =
            Vector3.Distance(transform.position,
            player.position);

        bool canInteract =
            distance <= interactDistance;

        if (promptUI != null)
            promptUI.SetActive(canInteract && !inPuzzle);

        if (canInteract &&
            Input.GetKeyDown(interactKey))
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

        yield return StartCoroutine(Fade(1));

        yield return StartCoroutine(MoveCamera(
            cameraViewPoint.position,
            cameraViewPoint.rotation,
            2f));

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

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        puzzleUI.SetActive(false);

        yield return StartCoroutine(Fade(1));

        yield return StartCoroutine(MoveCamera(
            originalCamPos,
            originalCamRot,
            2f));

        yield return StartCoroutine(Fade(0));

        if (playerController != null)
            playerController.enabled = true;

        inPuzzle = false;
        isTransitioning = false;
    }

    public void ToggleCamera()
    {
        isFixed = !isFixed;

        StopAllCoroutines();

        if (isFixed)
        {
            thirdPersonCam.enabled = false;

            StartCoroutine(MoveCamera(
                originalCamPos,
                originalCamRot,
                2f));
        }
        else
        {
            thirdPersonCam.enabled = true;
        }
    }

    IEnumerator MoveCamera(
        Vector3 targetPos,
        Quaternion targetRot,
        float duration)
    {
        float t = 0;

        Vector3 startPos =
            playerCamera.position;

        Quaternion startRot =
            playerCamera.rotation;

        while (t < duration)
        {
            playerCamera.position =
                Vector3.Lerp(
                    startPos,
                    targetPos,
                    t / duration);

            playerCamera.rotation =
                Quaternion.Slerp(
                    startRot,
                    targetRot,
                    t / duration);

            t += Time.deltaTime;

            yield return null;
        }

        playerCamera.position = targetPos;
        playerCamera.rotation = targetRot;
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha =
            fadeImage.color.a;

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime
                * fadeSpeed;

            Color c = fadeImage.color;

            c.a = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                t);

            fadeImage.color = c;

            yield return null;
        }
    }

    // =========================
    // PUZZLE SAVE SYSTEM
    // =========================

    public void AddPart(GameObject part)
    {
        if (!insertedParts.Contains(part.name))
        {
            insertedParts.Add(part.name);

            SavePuzzle();

            Debug.Log("Saved part: " + part.name);
        }
    }

    void SavePuzzle()
    {
        if (string.IsNullOrEmpty(puzzleID))
            return;

        if (GameManager.Instance == null)
            return;

        GameManager.Instance.savedPuzzleParts[puzzleID] =
            new List<string>(insertedParts);
    }

    void LoadPuzzle()
    {
        if (string.IsNullOrEmpty(puzzleID))
            return;

        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.savedPuzzleParts
            .ContainsKey(puzzleID))
        {
            insertedParts =
                new List<string>(
                    GameManager.Instance
                    .savedPuzzleParts[puzzleID]);

            foreach (string partName in insertedParts)
            {
                RestorePart(partName);
            }
        }
    }

    void RestorePart(string partName)
    {
        foreach (GameObject part in requiredParts)
        {
            if (part != null &&
                part.name == partName)
            {
                part.SetActive(true);

                Debug.Log(
                    "Restored part: " + partName);
            }
        }
    }
}