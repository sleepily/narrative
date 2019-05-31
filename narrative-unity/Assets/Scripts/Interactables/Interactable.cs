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

    private void OnMouseOver()
    {
        if (PlayerAimInteraction.IsFocusable(this))
            if (lastMouseAction != LastMouseAction.OnMouseOver)
            {
                Focus();
                lastMouseAction = LastMouseAction.OnMouseOver;
            }
    }

    private void OnMouseDown()
    {
        if (PlayerAimInteraction.IsFocusable(this))
        {
            if (lastMouseAction != LastMouseAction.OnMouseDown)
            {
                if (Input.GetMouseButtonDown(0))
                    Interact();
                if (Input.GetMouseButtonDown(1))
                    Search();

                lastMouseAction = LastMouseAction.OnMouseDown;
            }
        }
    }

    private void OnMouseExit()
    {
        if (lastMouseAction != LastMouseAction.OnMouseExit)
        {
            Unfocus();
            lastMouseAction = LastMouseAction.OnMouseExit;
        }
    }


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

    public void Search()
    {
        EventManager.Global.TriggerEvent(name, gameObject, "search");
    }
}
