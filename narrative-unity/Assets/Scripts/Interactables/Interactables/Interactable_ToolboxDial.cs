using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_ToolboxDial : Interactable
{
    public Puzzle_Toolbox puzzle;
    public Vector3 rotationAxis = Vector3.right;

    public int currentIcon = 0;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        TurnToCurrentIcon();
    }

    public override void Interact()
    {
        base.Interact();

        TurnForward();
    }

    void TurnForward()
    {
        currentIcon = ((currentIcon + 1) > 4) ? 0 : currentIcon + 1;
        TurnToCurrentIcon();
        puzzle.PuzzleCheck();
    }

    void TurnToCurrentIcon() => transform.localEulerAngles = rotationAxis * currentIcon * 90f;
}
