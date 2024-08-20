using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NextLevel : MonoBehaviour
{
    public TMP_Text displayText; 
    public Image backgroundImage; 
    public float displayDuration = 3f; 
    public float fadeDuration = 1f; 
    public string message = "Mensaje predeterminado";

    private bool isActive = false; 

    private void Start()
    {
        if (displayText != null)
            displayText.gameObject.SetActive(false);
        if (backgroundImage != null)
            backgroundImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive) 
        {
            StartCoroutine(ShowText(message));
        }
    }

    private IEnumerator ShowText(string text)
    {
        isActive = true; 

        if (displayText != null)
        {
            displayText.text = text;
            displayText.gameObject.SetActive(true); 
        }
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(true);
        }

        yield return StartCoroutine(FadeIn(displayText, backgroundImage, fadeDuration)); 

        yield return new WaitForSeconds(displayDuration); 

        yield return StartCoroutine(FadeOut(displayText, backgroundImage, fadeDuration)); 

        isActive = false; 
    }

    private IEnumerator FadeIn(TMP_Text text, Image image, float duration)
    {
        Color textColor = text.color;
        Color imageColor = image.color;
        for (float t = 0.01f; t < duration; t += Time.deltaTime)
        {
            float blend = t / duration;
            textColor.a = Mathf.Lerp(0, 1, blend);
            imageColor.a = Mathf.Lerp(0, 1, blend);
            text.color = textColor;
            image.color = imageColor;
            yield return null;
        }
        textColor.a = 1;
        imageColor.a = 1;
        text.color = textColor;
        image.color = imageColor;
    }

    private IEnumerator FadeOut(TMP_Text text, Image image, float duration)
    {
        Color textColor = text.color;
        Color imageColor = image.color;
        for (float t = 0.01f; t < duration; t += Time.deltaTime)
        {
            float blend = t / duration;
            textColor.a = Mathf.Lerp(1, 0, blend);
            imageColor.a = Mathf.Lerp(1, 0, blend);
            text.color = textColor;
            image.color = imageColor;
            yield return null;
        }
        textColor.a = 0;
        imageColor.a = 0;
        text.color = textColor;
        image.color = imageColor;
        text.gameObject.SetActive(false); 
        image.gameObject.SetActive(false);  
    }
}

