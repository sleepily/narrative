using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Sun : MonoBehaviour
{
    new Light light;
    float initialIntensity;
    public bool enableBlend = false;

    [Range(0, 1)] public float blend = 0f;

    public float lerpFactor = 3f;
    float targetBlend = 0f;

    public int blendSteps = 5;
    public int currentBlendStep = 0;

    private void Start()
    {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
    }

    public void ResetIntensity() => light.intensity = initialIntensity;
    
    public void AllowBlend(bool allowBlend) => enableBlend = allowBlend;

    public void IncreaseBlendStep()
    {
        if (++currentBlendStep > blendSteps)
            currentBlendStep = blendSteps;

        float newTargetBlend = Tools.ExtensionMethods.Map01(currentBlendStep, 0, blendSteps);

        SetTargetBlend(newTargetBlend);
    }

    public void SetTargetBlend(float targetBlend) =>
        this.targetBlend = Mathf.Clamp01(targetBlend);

    private void Update()
    {
        LerpToBlend();
    }

    void LerpToBlend()
    {
        if (!enableBlend)
            return;

        if (blend == targetBlend)
            return;

        blend = Mathf.Lerp
        (
            blend,
            targetBlend,
            Time.deltaTime / lerpFactor
        );

        light.intensity = initialIntensity - (blend * initialIntensity);
    }
}
