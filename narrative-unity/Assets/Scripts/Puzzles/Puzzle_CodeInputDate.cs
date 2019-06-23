using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CodeInputDate : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "date";

        base.OnEnableFunctions();
    }
}
