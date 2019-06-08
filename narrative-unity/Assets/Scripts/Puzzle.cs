using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    protected bool isSolved = false;

    /*
     * Separate virtual Start and Update functions to allow easier subclass specific actions
     */
    private void Start() => StartFunctions();

    protected virtual void StartFunctions() => GetAllComponents();

    protected virtual void GetAllComponents() { }

    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() { }

    /*
     * Puzzle specific functions for resetting, checking and solving
     */
    public virtual void PuzzleReset() { }

    public virtual void PuzzleCheck() { }

    public virtual bool PuzzleSolved() => true;
}
