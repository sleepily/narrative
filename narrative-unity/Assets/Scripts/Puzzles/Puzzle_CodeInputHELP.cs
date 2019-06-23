using UnityEngine;
using System.Collections;

public class Puzzle_CodeInputHELP : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "HELP";

        EventManager.Global.StartListening("Puzzle_CodeInput HELP", EventFunction);
    }
}
