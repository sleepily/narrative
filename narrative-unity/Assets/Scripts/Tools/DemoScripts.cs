using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.LWRP;

public class DemoScripts : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;

    AutoExposure autoExposure;
    ColorGrading colorGrading;

    public KeyCode reloadKey = KeyCode.R;

    // Start is called before the first frame update
    void Start()
    {
        if (postProcessVolume)
        {
            postProcessVolume.profile.TryGetSettings(out colorGrading);
            postProcessVolume.profile.TryGetSettings(out autoExposure);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
            GameManager.GLOBAL.sceneLoader.ReloadScene();

        if (postProcessVolume)
        {
            float exposureChange = Input.GetKeyDown(KeyCode.LeftBracket) ? -1f : Input.GetKeyDown(KeyCode.RightBracket) ? 1f : 0f;
            autoExposure.keyValue.value += exposureChange / 5;
        }
    }
}
