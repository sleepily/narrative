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
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        postProcessVolume.profile.TryGetSettings(out autoExposure);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            GameManager.GLOBAL.sceneLoader.LoadScene(0);
        }

        float scrollDelta = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scrollDelta) > float.Epsilon)
        {
            // colorGrading.postExposure.value += scrollDelta / 5;
            autoExposure.keyValue.value += scrollDelta / 5;
        }
    }
}
