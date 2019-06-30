using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CodeInput : Puzzle
{
    public Interactable_CodeInput.CodeType codeType;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        solution = solution.Trim();
    }

    public override bool PuzzleCheck(string playerInput)
    {
        playerInput = playerInput.Trim();

        bool answerIsCorrect = playerInput.Equals(solution);

        if (answerIsCorrect)
            PuzzleSolved();
        else
            TriggerDialogue();

        return answerIsCorrect;
    }
}
