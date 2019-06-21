using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_HeartLock : InteractableWithDialogue
{
    [Tooltip("Water bottle (will be unfocusable before unlock)")]
    public Item waterBottle;

    [Tooltip("The glass doors animator that will open once unlocked")]
    public Animator glassAnimator;

    public void Solve()
    {
        waterBottle.isFocusable = true;
        glassAnimator.SetBool("isSolved", true);
    }
}
