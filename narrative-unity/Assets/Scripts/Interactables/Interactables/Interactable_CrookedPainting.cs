using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Interactable_CrookedPainting : Interactable
{
    Puzzle_CrookedPainting puzzle;

    public bool isStraight = false;

    Animator animator;

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        puzzle = GetComponentInParent<Puzzle_CrookedPainting>();
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        base.Interact();

        if (isStraight)
        {
            puzzle.PuzzleSolvedReminder();
            return;
        }
    }

    public void SetStraight()
    {
        isStraight = true;
        animator.SetBool("isStraight", isStraight);
    }

    public void SolvePuzzle()
    {
        TriggerDialogue();
        puzzle.PuzzleSolved();
    }
}
