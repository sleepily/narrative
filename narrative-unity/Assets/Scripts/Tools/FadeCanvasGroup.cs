using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvasGroup : MonoBehaviour
{
    Image[] images;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
    }

    public void FadeToAlpha(float alpha)
    {
        StartCoroutine(Coroutine_Fade(1f, alpha, 1f));
    }

    IEnumerator Coroutine_Fade(float from, float to, float t)
    {
        float startTime = Time.time;
        float endTime = startTime + t;

        Color newColor;

        while (Time.time < endTime)
        {
            float mapped = Tools.ExtensionMethods.Map(Time.time, startTime, endTime, from, to);

            newColor = Color.white;
            newColor.a = mapped;

            foreach (Image image in images)
                image.color = newColor;

            yield return null;
        }

        newColor = Color.white;
        newColor.a = to;

        foreach (Image image in images)
            image.color = newColor;
    }
}
