using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    public void FadeToBlack()
    {
        StartCoroutine(FadeRoutine(0f, 1f));
    }
    public void FadeFromBlack()
    {
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    public IEnumerator FadeToBlackRoutine()
    {
        yield return FadeRoutine(0f, 1f);
    }
    public IEnumerator FadeFromBlackRoutine()
    {
        yield return FadeRoutine(1f, 0f);
    }

    IEnumerator FadeRoutine(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        
        // Ensure the image is active
        if (!fadeImage.gameObject.activeSelf)
        {
            fadeImage.gameObject.SetActive(true);
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null; // Wait until the next frame
        }

        // Ensure the final alpha is set precisely
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, endAlpha);

        // Optional: Deactivate the image if fully transparent to save rendering performance
        if (endAlpha == 0f)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
}
