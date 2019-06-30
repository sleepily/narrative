using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Puzzle_DoorWithKnobs : Puzzle
{
    public int rotationSteps = 8;
    public GameObject doorKnobParent;

    List<Interactable_Doorknob> doorknobs;
    int[] solutionArray;

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        SetUpDoorknobs();
        solutionArray = FormatSolution();
    }

    void SetUpDoorknobs()
    {
        doorknobs = doorKnobParent.GetComponentsInChildren<Interactable_Doorknob>().ToList();
        doorknobs = doorknobs.OrderBy(knob => knob.name).ToList();

        foreach (Interactable_Doorknob knob in doorknobs)
            knob.SetPuzzle(this);
    }

    int[] FormatSolution()
    {
        int[] output = new int[doorknobs.Count];

        for (int knobIndex = 0; knobIndex < doorknobs.Count; knobIndex++)
        {
            int.TryParse(solution[knobIndex].ToString(), out output[knobIndex]);
            output[knobIndex] %= rotationSteps;
        }

        return output;
    }

    public override bool PuzzleCheck()
    {
        bool isCorrect = true;

        for (int knobIndex = 0; knobIndex < doorknobs.Count; knobIndex++)
            if (solutionArray[knobIndex] != doorknobs[knobIndex].rotationStep)
                isCorrect = false;

        // Don't trigger any dialogue
        if (!isCorrect)
            return false;

        PuzzleSolved();
        return true;
    }
}
