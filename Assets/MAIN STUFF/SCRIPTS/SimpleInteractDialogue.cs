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

    //stores the players camera abd tracks when dialouge is shown
    private Transform cam;
    private bool isShowing = false;

    //finds camera labeled main camera and stores position and direction
    //turns UI off at the start
    //sets the prompt text in ui
    void Start()
    {
        cam = Camera.main.transform;

        dialogueUI.SetActive(false);
        promptUI.SetActive(false);

        promptText.text = promptMessage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowDialogue();
        }
    }

    //void Update()
    //{

    //    //creats a raycast and starts at camera position
    //    //shoots where player is looking
    //    Ray ray = new Ray(cam.position, cam.forward);
    //    RaycastHit hit;

    //    //PLAYER IS NOTTTT LOOKING AT OBJECT
    //    bool isLookingAtObject = false;

    //    //shoots ray foward and if it hits something thenn continuie
    //    if (Physics.Raycast(ray, out hit, interactDistance))
    //    {
    //        //checks if the object was hit and if yes then player is looking at it
    //        if (hit.transform != null)
    //        {
    //            isLookingAtObject = true;
    //        }
    //    }

    //    // Show prompt only if looking AND not in dialogue
    //    if (isLookingAtObject && !isShowing)
    //    {
    //        //turns prompt on
    //        promptUI.SetActive(true);

    //        //if press e then open dialouge 
    //        if (Input.GetKeyDown(interactKey))
    //        {
    //            ShowDialogue();
    //        }
    //    }
    //    //if not looking thern hide prompt
    //    else
    //    {
    //        promptUI.SetActive(false);
    //    }

    //    // Close dialogue on click
    //    if (isShowing && Input.GetMouseButtonDown(0))
    //    {
    //        HideDialogue();
    //    }
    //}

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