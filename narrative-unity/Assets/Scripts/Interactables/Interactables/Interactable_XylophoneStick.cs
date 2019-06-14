using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_XylophoneStick : Interactable
{
    public override void Interact()
    {
        base.Interact();

        EventManager.Global.TriggerEvent("Puzzle_Xylophone", this.gameObject);
    }
}
