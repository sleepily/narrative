using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (PlayerAimInteraction.IsFocusable(this))
            Focus();
    }

    private void OnMouseDown()
    {
        if (PlayerAimInteraction.IsFocusable(this))
        {
            if (Input.GetMouseButtonDown(0))
                Interact();
            if (Input.GetMouseButtonDown(1))
                Search();
        }
    }

    private void OnMouseExit()
    {
        Unfocus();
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
