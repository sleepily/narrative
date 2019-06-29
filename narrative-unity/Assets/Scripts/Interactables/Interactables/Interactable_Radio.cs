using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Radio : InteractableWithDialogue
{
    public Interactable_Clock clock;

    public void CheckTime()
    {
        // Do something with
        clock.GetTime().ToShortTimeString();
    }
}
