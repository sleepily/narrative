using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CodeInputDigits : Puzzle_CodeInput
{
    protected override void OnEnableFunctions()
    {
        codeType = "digits";

        base.OnEnableFunctions();
    }
}
