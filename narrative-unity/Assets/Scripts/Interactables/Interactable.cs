using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    protected Glowable glowable;

    /*
     * OnMouse functions to determine whether to focus, unfocus, interact with or use an object
     */
    private void OnMouseEnter() => MouseButtonCheck();

    private void OnMouseOver() => MouseButtonCheck();

    private void OnMouseExit()
    {
        Unfocus();
    }

    /*
     * Manual mouse button check, since the collider's OnMouse() function
     * only takes primary mouse button clicks into account
     */
    void MouseButtonCheck()
    {
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
    public virtual void StartFunctions() => GetAllComponents();
    protected virtual void GetAllComponents() => GetGlowable();

    void GetGlowable()
    {
        glowable = GetComponent<Glowable>();
    }

    /*
     * Use virtual void in Update for inheritance compatibility
     */
    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() { }

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
    public virtual void Focus()
    {
        if (glowable)
            glowable.SetGlowColorGlow();
    }

    public virtual void Unfocus()
    {
        if (glowable)
            glowable.SetGlowColorClear();
    }

    public virtual void Interact() { }

    public virtual void Use() { }
}
