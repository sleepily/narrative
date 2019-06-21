using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CodeInput : Puzzle
{
    protected string codeType = "";
    string playerInput;

    private void OnEnable()
    {
        OnEnableFunctions();
    }

    protected virtual void OnEnableFunctions()
    {
        EventManager.Global.StartListening("Puzzle_CodeInput " + codeType, EventFunction);
    }

    protected override void StartFunctions()
    {
        base.StartFunctions();

        solution = solution.Trim();
    }

    public void SetPlayerInput(string inputString)
    {
        playerInput = inputString.Trim();
    }

    public void EventFunction(GameObject sender, string playerAnswer = "")
    {
        SetPlayerInput(playerAnswer);
        PuzzleCheck();
    }

    public override bool PuzzleCheck()
    {
        bool answerIsCorrect = playerInput.Equals(solution);

        if (answerIsCorrect)
            PuzzleSolved();
        else
            TriggerDialogue();

        return answerIsCorrect;
    }
}
