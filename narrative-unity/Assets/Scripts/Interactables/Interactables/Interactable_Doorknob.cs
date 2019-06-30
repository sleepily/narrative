using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Doorknob : Interactable
{
    public int rotationStep = 0;

    Vector3 rotationAxis = Vector3.forward;

    Puzzle_DoorWithKnobs puzzle;

    public override void Interact()
    {
        base.Interact();

        IncreaseStep();
        RotateForward();
        puzzle.PuzzleCheck();
    }

    public void RotateForward()
    {
        Vector3 newAngle = rotationAxis * rotationStep * (360f / puzzle.rotationSteps);
        transform.localRotation = Quaternion.Euler(newAngle);
    }

    void IncreaseStep()
    {
        if (++rotationStep >= puzzle.rotationSteps)
            rotationStep = 0;
    }

    public void SetPuzzle(Puzzle_DoorWithKnobs puzzle) => this.puzzle = puzzle;
}
