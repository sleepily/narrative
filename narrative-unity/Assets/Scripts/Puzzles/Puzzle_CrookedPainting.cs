using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CrookedPainting : Puzzle
{
    public Item_Magazine magazine;

    public override void PuzzleSolved()
    {
        base.PuzzleSolved();

        magazine.PickupItem();
    }
}
