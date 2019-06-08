using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Interactable : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    protected Color glowColor = Color.yellow;
    protected Color currentGlowColor, desiredGlowColor;

    protected float colorLerpFactor = .2f;
    protected bool lerpIsFinished = true;

    // check for last mouse action to avoid spam of commands on every frame
    enum LastMouseAction
    {
        OnMouseEnter,
        OnMouseOver,
        OnMouseDown,
        OnMouseExit
    }

    LastMouseAction lastMouseAction = LastMouseAction.OnMouseExit;

    /*
     * OnMouse functions to determine whether to focus, unfocus, interact with or use an object
     */
    private void OnMouseEnter() => MouseButtonCheck();

    private void OnMouseOver() => MouseButtonCheck();

    private void OnMouseExit()
    {
        if (lastMouseAction != LastMouseAction.OnMouseExit)
        {
            Unfocus();
            lastMouseAction = LastMouseAction.OnMouseExit;
        }
    }

    /*
     * Manual mouse button check, since the collider's OnMouse() function
     * only takes primary mouse button clicks into account
     */
    void MouseButtonCheck()
    {
        if (PlayerAimInteraction.IsFocusable(this))
        {
            if (lastMouseAction != LastMouseAction.OnMouseDown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastMouseAction = LastMouseAction.OnMouseDown;
                    Interact();

                    return;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    lastMouseAction = LastMouseAction.OnMouseDown;
                    Use();

                    return;
                }
            }

            if (lastMouseAction != LastMouseAction.OnMouseOver)
            {
                Focus();
                lastMouseAction = LastMouseAction.OnMouseOver;

                return;
            }
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

    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() => LerpGlowColor();

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

    protected void SetGlowColor(Color color, bool isInstantTransition = false)
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

    /*
     * All possible interaction functions which are defined in the subclasses
     */
    public virtual void Focus() => SetGlowColor(glowColor);

    public virtual void Unfocus() => SetGlowColor(Color.clear);

    public virtual void Interact() { }

    public virtual void Use() { }
}
