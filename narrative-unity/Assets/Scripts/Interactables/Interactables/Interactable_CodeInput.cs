using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_CodeInput : Interactable
{
    public Puzzle_CodeInput puzzle;

    public enum CodeType
    {
        digits,
        DeskDrawer,
        MILK,
        TAYLOR,
        Toolbox,
        RadioFrequency
    }

    public CodeType codeType;

    public UI_CodeInput codeInput;

    public override void Interact()
    {
        if (puzzle.isSolved)
        {
            puzzle.PuzzleSolvedReminder();
            return;
        }

        if (puzzle.IsInDialogueCheck())
            return;

        codeInput.ToggleVisibility(setVisible: true);
    }
}
