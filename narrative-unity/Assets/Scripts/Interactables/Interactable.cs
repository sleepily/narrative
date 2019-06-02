using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
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
    private void OnMouseEnter()
    {
        MouseButtonCheck();
    }

    private void OnMouseOver()
    {
        MouseButtonCheck();
    }

    private void OnMouseExit()
    {
        if (lastMouseAction != LastMouseAction.OnMouseExit)
        {
            Unfocus();
            lastMouseAction = LastMouseAction.OnMouseExit;
        }
    }

    void MouseButtonCheck()
    {
        if (PlayerAimInteraction.IsFocusable(this))
        {
            if (lastMouseAction != LastMouseAction.OnMouseOver)
            {
                Focus();
                lastMouseAction = LastMouseAction.OnMouseOver;
            }

            if (lastMouseAction != LastMouseAction.OnMouseDown)
            {
                bool buttonDown = false;

                if (Input.GetAxisRaw("Interact") > float.Epsilon)
                {
                    buttonDown = true;
                    Interact();
                }
                if (Input.GetAxisRaw("UseItem") > float.Epsilon)
                {
                    buttonDown = true;
                    Use();
                }

                if (buttonDown)
                    lastMouseAction = LastMouseAction.OnMouseDown;
            }
        }
        else
            OnMouseExit();
    }

    /*
     * All possible world object/item interactions
     */
    public void Focus()
    {
        EventManager.Global.TriggerEvent(name, gameObject, "focus");
    }

    public void Unfocus()
    {
        EventManager.Global.TriggerEvent(name, gameObject, "unfocus");
    }

    public void Interact()
    {
        EventManager.Global.TriggerEvent(name, gameObject, "interact");
    }

    public void Use()
    {
        EventManager.Global.TriggerEvent(name, gameObject, "use");
    }
}
