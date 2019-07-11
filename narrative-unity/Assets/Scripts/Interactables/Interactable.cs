using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{

    [Header("Color Glow Components")]
    protected MeshRenderer meshRenderer;
    protected Color currentGlowColor, desiredGlowColor;

    [Header("Color Glow Properties")]
    protected Color glowColor = Color.yellow;
    protected float colorLerpFactor = .2f;
    protected bool lerpIsFinished = true;

    [Tooltip("Deselect in case this interactable should not be focusable.")]
    public bool isFocusable = true;
    public bool focusWhenPlayerIsNear = false;

    /*
     * OnMouse functions to determine whether to focus, unfocus, interact with or use an object
     */
    private void OnMouseEnter() => MouseButtonCheck();

    private void OnMouseOver() => MouseButtonCheck();

    private void OnMouseExit() => Unfocus();

    /*
     * Manual mouse button check, since the collider's OnMouse() function
     * only takes primary mouse button clicks into account
     */
    public virtual void MouseButtonCheck()
    {
        if (!isFocusable)
            return;

        if (PlayerAimInteraction.IsFocusable(this))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Interact();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Use();
                return;
            }

            Focus();
            return;
        }
        else
            OnMouseExit();
    }

    /*
     * Separate virtual Start and Update functions to allow easier subclass specific actions
     */
    private void Start() => StartFunctions();

    protected virtual void StartFunctions()
    {
        GetAllComponents();
        SetGlowColor(Color.clear, isInstantTransition: true);
    }

    protected virtual void GetAllComponents()
    {
        // Get components needed for color glow on hover
        meshRenderer = GetComponent<MeshRenderer>();
        glowColor = meshRenderer.material.GetColor("_GlowColor");
    }

    /*
     * Use virtual void in Update for inheritance compatibility
     */
    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions()
    {
        FocusIfNearPlayer();
        LerpGlowColor();
    }

    void FocusIfNearPlayer()
    {
        if (!focusWhenPlayerIsNear)
            return;

        if (!isFocusable)
            return;

        if (PlayerAimInteraction.IsFocusable(this))
            Focus();
    }

    /*
     * Constantly lerps to the desired glow color
     * No action is taken when the color lerp has finished
     */
    protected void LerpGlowColor()
    {
        if (lerpIsFinished)
            return;

        if (!meshRenderer)
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

    protected void SetGlowColor(Color color, bool isInstantTransition = false)
    {
        desiredGlowColor = color;
        lerpIsFinished = isInstantTransition;

        if (!isInstantTransition)
            return;

        currentGlowColor = desiredGlowColor;

        if (meshRenderer)
            meshRenderer.material.SetColor("_GlowColor", desiredGlowColor);
    }

    /*
     * In case only the glow color needs to be overriden
     */
    protected virtual void OverrideGlowColor(Color glowColorOverride) => glowColor = glowColorOverride;

    /*
     * Enable/Disable listening when objects are used/not used.
     */
    private void OnEnable() => OnEnableFunctions();

    private void OnDisable() => OnDisableFunctions();

    protected virtual void OnEnableFunctions() => EventManager.Global.StartListening(name, EventFunction);

    protected virtual void OnDisableFunctions() => EventManager.Global.StopListening(name, EventFunction);

    /*
     * EventFunction triggered through EventSystem to allow for global object communication
     */
    public virtual void EventFunction(GameObject sender, string parameter = "")
    {
        switch (parameter)
        {
            case "interact":
                Interact();
                break;
            case "use":
                Use();
                break;
            case "focus":
                Focus();
                break;
            case "unfocus":
                Unfocus();
                break;
            default:
                break;
        }
    }

    public void SetFocusable(bool focus = true)
    {
        isFocusable = focus;
    }

    /*
     * All possible interaction functions which are defined in the subclasses
     */
    public virtual void Focus() => SetGlowColor(glowColor);

    public virtual void Unfocus() => SetGlowColor(Color.clear);

    public virtual void Interact() { }

    public virtual void Use() { }
}