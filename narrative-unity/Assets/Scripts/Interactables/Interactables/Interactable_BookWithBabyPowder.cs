using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_BookWithBabyPowder : Interactable
{
    public Item babyPowder;

    public void Activate()
    {
        isFocusable = true;
    }

    public override void Interact()
    {
        base.Interact();

        babyPowder.PickupItem();
    }
}
