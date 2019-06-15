using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CodeInput : Puzzle
{
    public Interactable_CodeInput.CodeType codeType;

    int playerInput;
    int solutionInt;

    private void OnEnable()
    {
        EventManager.Global.StartListening("Puzzle_CodeInput " + codeType.ToString(), EventFunction);
    }

    protected override void StartFunctions()
    {
        base.StartFunctions();

        solution = solution.Trim();
        solutionInt = int.Parse(solution);
    }

    public void SetPlayerInput(int inputNumber)
    {
        playerInput = inputNumber;
    }

    public void EventFunction(GameObject sender, string playerAnswer = "")
    {
        SetPlayerInput(int.Parse(playerAnswer));
        PuzzleCheck();
    }

    public override bool PuzzleCheck()
    {
        bool answerIsCorrect = (playerInput == solutionInt);

        if (answerIsCorrect)
            PuzzleSolved();
        else
            TriggerDialogue();

        return answerIsCorrect;
    }

    public override void PuzzleSolved()
    {
        flowchart.SetBooleanVariable("isSolved", true);

        base.PuzzleSolved();
    }
}
