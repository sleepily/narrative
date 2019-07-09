using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FadeManager : MonoBehaviour
{
    PostProcessVolume postProcessVolume;
    AutoExposure autoExposure;

    public bool allowFadeOverride = true;

    bool fadeActive = false;

    private void Start()
    {
        GameManager.GLOBAL.postProcessVolume.profile.TryGetSettings(out autoExposure);

        ResetFade();
    }

    void ResetFade() => autoExposure.keyValue.value = 1f;

    public void FadeBlack(bool toBlack)
    {
        if (!allowFadeOverride)
            if (fadeActive)
                return;

        StartCoroutine(Coroutine_FadeToColor(toBlack));
    }

    IEnumerator Coroutine_FadeToColor(bool toBlack, float fadeTime = 1f)
    {
        fadeActive = true;

        float startFade = Time.time;
        float endFade = startFade + fadeTime;

        // fadeSprite.CrossFadeColor(color, fadeTime, ignoreTimeScale: true, useAlpha: true);

        while (Time.time < endFade)
        {
            float progress = Tools.ExtensionMethods.Map01(Time.time, startFade, endFade);

            if (toBlack)
                progress = 1 - progress;

            autoExposure.keyValue.value = progress;

            yield return null;
        }

        autoExposure.keyValue.value = toBlack ? 0 : 1;

        fadeActive = false;
    }
}
