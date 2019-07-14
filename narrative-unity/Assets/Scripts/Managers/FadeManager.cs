using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class FadeManager : MonoBehaviour
{
    public enum Exposures { Current, Black, Default, White }
    float[] exposures = { -1f, 0f, 1f, 600f};

    [Header("Scene")]
    [Tooltip("Will use Global PPV if null.")]
    public PostProcessVolume postProcessVolume;
    AutoExposure autoExposure;

    [Space]
    [Tooltip("Allow the interruption of one fade by another.")]
    public bool allowFadeOverride = true;

    [Header("Level")]
    public TextMeshProUGUI titleText;
    CanvasGroup canvasGroup;

    bool fadeActive = false;

    private void Start()
    {
        GetAllComponents();

        ResetFade();
    }

    void GetAllComponents()
    {
        if (titleText)
        {
            canvasGroup = titleText.GetComponentInParent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        }

        if (!postProcessVolume)
            postProcessVolume = GameManager.GLOBAL.postProcessVolume;

        if (!postProcessVolume.profile.TryGetSettings(out autoExposure))
            Debug.Log($"Couldn't get AutoExposure from {postProcessVolume.profile.name}");
    }

    void ResetFade() => autoExposure.keyValue.value = 1f;

    public void Fade(Exposures from, Exposures to, float fadeTime = 1f) =>
        Fade(exposures[(int)from], exposures[(int)to], fadeTime);

    public void Fade(float fromExposure, float toExposure, float fadeTime = 1f)
    {
        if (fadeActive)
        {
            if (!allowFadeOverride)
                return;
            if (allowFadeOverride)
                StopAllCoroutines();
        }

        fromExposure = (fromExposure < 0) ? autoExposure.keyValue.value : fromExposure;
        toExposure = (toExposure < 0) ? autoExposure.keyValue.value : toExposure;

        if (!autoExposure)
            GetAllComponents();

        StartCoroutine(Coroutine_Fade(fromExposure, toExposure, fadeTime));
    }
    
    IEnumerator Coroutine_Fade(float fromExposure, float toExposure, float fadeTime = 1f)
    {
        fadeActive = true;

        float startFade = Time.time;
        float endFade = startFade + fadeTime;

        // fadeSprite.CrossFadeColor(color, fadeTime, ignoreTimeScale: true, useAlpha: true);

        while (Time.time < endFade)
        {
            float newExposure = Tools.ExtensionMethods.Map(Time.time, startFade, endFade, fromExposure, toExposure);
            float progress = Tools.ExtensionMethods.Map01(Time.time, startFade, endFade) * 100;

            autoExposure.keyValue.value = newExposure;
            // Debug.Log($"Fade: {progress}%");

            yield return null;
        }

        // Prevent rounding errors by overwriting after last iteration
        autoExposure.keyValue.value = toExposure;

        // Debug.Log($"Fade: 100%");

        fadeActive = false;
    }

    public void FadeToBlack(float fadeTime = 1f) =>
        Fade(Exposures.Current, Exposures.Black, fadeTime);

    public void FadeFromBlack(float fadeTime = 1f) =>
        Fade(Exposures.Black, Exposures.Default, fadeTime);

    public void FadeToWhite(float fadeTime = 1f) =>
        Fade(Exposures.Current, Exposures.White, fadeTime);

    public void FadeFromWhite(float fadeTime = 1f) =>
        Fade(Exposures.White, Exposures.Default, fadeTime);

    public void FadeToTitle(string title, float fadeInTime = 1f, float fadeTime = .6f) =>
        StartCoroutine(Coroutine_FadeTitle(title, fadeInTime, fadeTime, holdTime: 3f));

    IEnumerator Coroutine_FadeTitle(string title, float fadeInTime, float fadeTime, float holdTime)
    {
        Fade(Exposures.Default, Exposures.White, fadeInTime);
        yield return new WaitForSeconds(fadeInTime);

        string formattedTitle = title.Replace('|', '\n');
        titleText.text = formattedTitle;

        StartCoroutine(Coroutine_FadeCanvasGroup(0f, 1f, fadeInTime));
        yield return new WaitForSeconds(fadeTime);

        yield return new WaitForSeconds(holdTime);

        StartCoroutine(Coroutine_FadeCanvasGroup(1f, 0f, fadeTime));
        yield return new WaitForSeconds(fadeTime);

        Fade(Exposures.White, Exposures.Default, fadeTime );
        yield return new WaitForSeconds(fadeTime);
    }

    IEnumerator Coroutine_FadeCanvasGroup(float fromAlpha, float toAlpha, float fadeTime)
    {
        float startFade = Time.time;
        float endFade = startFade + fadeTime;

        while (Time.time < endFade)
        {
            float alpha = Tools.ExtensionMethods.Map(Time.time, startFade, endFade, fromAlpha, toAlpha);

            canvasGroup.alpha = alpha;

            yield return null;
        }

        canvasGroup.alpha = toAlpha;
    }
}
