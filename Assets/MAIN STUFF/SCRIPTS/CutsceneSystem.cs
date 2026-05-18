using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneSystem : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneSlide
    {
        [TextArea(2, 6)]
        public string text;

        public Sprite image;
    }

    [Header("Cutscene")]
    public CutsceneSlide[] slides;

    [Header("UI")]
    public GameObject cutscenePanel;
    public Image sceneImage;
    public TextMeshProUGUI dialogueText;
    public GameObject continueIcon;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Next Scene")]
    public string nextSceneName = "Game";

    private int currentSlide = 0;
    private bool isTyping = false;
    private bool canContinue = false;

    void Start()
    {
        cutscenePanel.SetActive(true);
        StartCoroutine(ShowSlide());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();

                dialogueText.text = slides[currentSlide].text;

                isTyping = false;
                canContinue = true;

                if (continueIcon != null)
                    continueIcon.SetActive(true);
            }
            else if (canContinue)
            {
                NextSlide();
            }
        }
    }

    IEnumerator ShowSlide()
    {
        canContinue = false;
        isTyping = true;

        if (continueIcon != null)
            continueIcon.SetActive(false);

        sceneImage.sprite = slides[currentSlide].image;

        dialogueText.text = "";

        foreach (char letter in slides[currentSlide].text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canContinue = true;

        if (continueIcon != null)
            continueIcon.SetActive(true);
    }

    void NextSlide()
    {
        currentSlide++;

        if (currentSlide >= slides.Length)
        {
            EndCutscene();
            return;
        }

        StartCoroutine(ShowSlide());
    }

    void EndCutscene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}