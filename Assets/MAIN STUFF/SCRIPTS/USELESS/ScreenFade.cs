using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;

    public Image fadeImage;

    public float fadeSpeed = 2f;

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut()
    {
        Color c = fadeImage.color;

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeSpeed;

            fadeImage.color = c;

            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    public IEnumerator FadeIn()
    {
        Color c = fadeImage.color;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;

            fadeImage.color = c;

            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}