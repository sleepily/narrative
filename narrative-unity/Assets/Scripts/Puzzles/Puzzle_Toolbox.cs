using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Toolbox : Puzzle
{
    Interactable_ToolboxDial[] dials = new Interactable_ToolboxDial[4];
    int[] solutionArray = new int[4];

    public GameObject dialsParent;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        GetDials();
        CreateSolutionArray();
    }

    void GetDials()
    {
        dials = dialsParent.GetComponentsInChildren<Interactable_ToolboxDial>();
    }

    void CreateSolutionArray()
    {
        int index = 0;
        foreach (char digit in solution.ToCharArray())
        {
            int intToAdd;

            if (!int.TryParse(digit.ToString(), out intToAdd))
                intToAdd = 0;

            solutionArray[index] = intToAdd;
            index++;
        }
    }

    public override bool PuzzleCheck()
    {
        for (int index = 0; index < solutionArray.Length; index++)
        {
            bool isEqual = (solutionArray[index] == dials[index].currentIcon);

            if (!isEqual)
                return false;
        }

        PuzzleSolved();
        return true;
    }
}
