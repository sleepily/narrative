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

    public List<TeleportLocation> teleportLocations;

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

        int teleportIndex = -1;

        if (Input.GetKeyDown(KeyCode.Z))
            teleportIndex = 0;
        if (Input.GetKeyDown(KeyCode.X))
            teleportIndex = 1;
        if (Input.GetKeyDown(KeyCode.C))
            teleportIndex = 2;

        if (teleportIndex >= 0)
        {
            TeleportLocation destination = teleportLocations[teleportIndex];
            GameManager.GLOBAL.player.teleportPlayer.TeleportIntoLevel(destination);
        }

        if (postProcessVolume)
        {
            float exposureChange = Input.GetKeyDown(KeyCode.LeftBracket) ? -1f : Input.GetKeyDown(KeyCode.RightBracket) ? 1f : 0f;
            autoExposure.keyValue.value += exposureChange / 5;
        }
    }
}
