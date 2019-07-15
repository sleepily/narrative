using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using FMOD;

public class SettingsManager : MonoBehaviour
{
    public PostProcessProfile postprocessProfile;
    ColorGrading colorGrading;

    FMOD.Studio.Bus fmodMaster;
    string masterBusString = "Bus:/";

    float gamma = 1f;
    float music = 1f;

    private void Start()
    {
        GetPostProcessing();
        GetFMOD();
        GetPlayerPrefs();
    }

    void GetPostProcessing()
    {
        if (postprocessProfile)
            postprocessProfile.TryGetSettings(out colorGrading);
    }

    void GetFMOD()
    {
        fmodMaster = FMODUnity.RuntimeManager.GetBus(masterBusString);
    }

    void GetPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("gamma"))
            gamma = PlayerPrefs.GetFloat("gamma");

        if (PlayerPrefs.HasKey("music"))
            music = PlayerPrefs.GetFloat("music");
    }

    public void SetGamma(float value)
    {
        gamma = value;
        UpdateSettings();
    }

    public void SetMusic(float value)
    {
        music = value;
        UpdateSettings();
    }

    void UpdateSettings()
    {
        fmodMaster.setVolume(music);
        // colorGrading.gamma.value = gamma;
    }
}
