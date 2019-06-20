using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowable : MonoBehaviour
{
    [Header("Color Glow Components")]
    protected MeshRenderer meshRenderer;
    protected Color currentGlowColor, desiredGlowColor;

    [Header("Color Glow Properties")]
    public Color glowColor = Color.yellow;
    public float colorLerpFactor = .2f;
    protected bool lerpIsFinished = true;

    private void Start() => StartFunctions();

    public virtual void StartFunctions()
    {
        GetAllComponents();
        SetGlowColor(Color.clear, isInstantTransition: true);
    }

    private void Update() => UpdateFunctions();

    public virtual void UpdateFunctions() => LerpGlowColor();

    protected virtual void GetAllComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        glowColor = meshRenderer.material.GetColor("_GlowColor");
    }

    /*
     * Constantly lerps to the desired glow color
     * No action is taken when the color lerp has finished
     */
    protected void LerpGlowColor()
    {
        if (lerpIsFinished)
            return;

        currentGlowColor = Color.Lerp
        (
            currentGlowColor,
            desiredGlowColor,
            Time.deltaTime / colorLerpFactor
        );

        meshRenderer.material.SetColor("_GlowColor", currentGlowColor);

        if (currentGlowColor.Equals(desiredGlowColor))
            lerpIsFinished = true;
    }

    public void SetGlowColorGlow()
    {
        SetGlowColor(glowColor);
    }

    public void SetGlowColorClear()
    {
        SetGlowColor(Color.clear);
    }

    public void SetGlowColor(Color color, bool isInstantTransition = false)
    {
        desiredGlowColor = color;
        lerpIsFinished = isInstantTransition;

        if (!isInstantTransition)
            return;

        currentGlowColor = desiredGlowColor;
        meshRenderer.material.SetColor("_GlowColor", desiredGlowColor);
    }

    /*
     * In case only the glow color needs to be overriden
     */
    public virtual void OverrideGlowColor(Color glowColorOverride) => glowColor = glowColorOverride;

}
