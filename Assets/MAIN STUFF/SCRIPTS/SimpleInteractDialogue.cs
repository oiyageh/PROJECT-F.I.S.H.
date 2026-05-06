using UnityEngine;
using TMPro;

public class SimpleInteractDialogue : MonoBehaviour
{
    //dialougeUI put in inspector, panel for dialouge and dialougetext is the actual text
    [Header("UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    //propmt e contatiner and the text
    public GameObject promptUI;
    public TextMeshProUGUI promptText;

    //the message that will play
    [Header("Dialogue")]
    [TextArea]
    public string message = "Hello there!";

    //when player is staring at the object
    [Header("Prompt")]
    public string promptMessage = "Press E to interact";

    //inspcetor adjustments, change the key and distance can be interacted with
    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 3f;

    //stores the players position instead and tracks when dialouge is shown
    private Transform player;
    private bool isShowing = false;

    //finds camera labeled main camera and stores position and direction
    //turns UI off at the start
    //sets the prompt text in ui
    void Start()
    {
        player = Camera.main.transform;

        dialogueUI.SetActive(false);
        promptUI.SetActive(false);

        promptText.text = promptMessage;
    }

   

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // If close enough and NOT already talking
        if (distance <= interactDistance && !isShowing)
        {
            promptUI.SetActive(true);

            if (Input.GetKeyDown(interactKey))
            {
                ShowDialogue();
            }
        }
        else
        {
            promptUI.SetActive(false);
        }

        // Close dialogue with mouse click
        if (isShowing && Input.GetMouseButtonDown(0))
        {
            HideDialogue();
        }
    }

    //show dialouge ui
    void ShowDialogue()
    {
        dialogueUI.SetActive(true);
        //set the messaqge text
        dialogueText.text = message;

        //hide prompt and mark dialouge as active
        promptUI.SetActive(false);
        isShowing = true;
    }

    //hide ui and reset state
    void HideDialogue()
    {
        dialogueUI.SetActive(false);
        isShowing = false;
    }
}