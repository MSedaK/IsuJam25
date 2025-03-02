using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaAnimation : MonoBehaviour
{
    public GameObject[] panels; // UI panelleri (Panel i�inde Image, Text vs. olabilir)

    private void Start()
    {
        StartCoroutine(FadeInPanels());
    }

    IEnumerator FadeInPanels()
    {
        float duration = 1f; // Ge�i� s�resi
        float delayStep = 0.2f; // Her panel i�in gecikme

        for (int i = 0; i < panels.Length; i++)
        {
            StartCoroutine(FadeIn(panels[i], duration));
            yield return new WaitForSeconds(delayStep);
        }
    }

    IEnumerator FadeIn(GameObject panel, float duration)
    {
        Graphic[] graphics = panel.GetComponentsInChildren<Graphic>(); // UI elemanlar�n� al
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            foreach (var graphic in graphics)
            {
                Color color = graphic.color;
                color.a = Mathf.Lerp(0, 1, time / duration);
                graphic.color = color;
            }
            yield return null;
        }
    }

    public void FadeOutAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            StartCoroutine(FadeOut(panel, 0.5f));
        }
    }

    IEnumerator FadeOut(GameObject panel, float duration)
    {
        Graphic[] graphics = panel.GetComponentsInChildren<Graphic>();
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            foreach (var graphic in graphics)
            {
                Color color = graphic.color;
                color.a = Mathf.Lerp(1, 0, time / duration);
                graphic.color = color;
            }
            yield return null;
        }
    }
}
