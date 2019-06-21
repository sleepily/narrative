using UnityEngine;
using System.Collections;

public class Puzzle_CodeInputText : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "text";

        base.OnEnableFunctions();
    }
}
