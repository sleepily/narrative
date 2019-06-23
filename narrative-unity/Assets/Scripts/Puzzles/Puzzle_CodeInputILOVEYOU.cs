using UnityEngine;
using System.Collections;

public class Puzzle_CodeInputILOVEYOU : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "ILOVEYOU";

        EventManager.Global.StartListening("Puzzle_CodeInput ILOVEYOU", EventFunction);
    }
}
