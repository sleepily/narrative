using UnityEngine;
using System.Collections;

public class Puzzle_CodeInputToolbox : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "toolbox";

        EventManager.Global.StartListening("Puzzle_CodeInput toolbox", EventFunction);
    }
}
