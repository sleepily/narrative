using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Interactable_CrookedPainting : Interactable
{
    public bool isStraight = false;

    Animator animator;

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        base.Interact();

        SetStraight();
    }

    void SetStraight()
    {
        isStraight = true;
        animator.SetBool("isStraight", isStraight);
    }
}
