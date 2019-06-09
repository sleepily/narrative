using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class Puzzle : MonoBehaviour
{
    protected bool isSolved = false;

    protected Flowchart flowchart;

    /*
     * Separate virtual Start and Update functions to allow easier subclass specific actions
     */
    private void Start() => StartFunctions();

    protected virtual void StartFunctions() => GetAllComponents();

    protected virtual void GetAllComponents()
    {
        GetFlowchart();
    }

    Flowchart GetFlowchart()
    {
        if (!flowchart)
            flowchart = GetComponent<Flowchart>();

        return flowchart;
    }

    public void TriggerDialogue()
    {
        if (!GetFlowchart())
        {
            Debug.LogWarning(name + ": Cannot trigger dialogue, Flowchart missing.");
            return;
        }

        flowchart.ExecuteBlock("Start");
    }

    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() { }

    /*
     * Puzzle specific functions for resetting, checking and solving
     */
    public virtual void PuzzleReset() { }

    public virtual void PuzzleCheck() { }

    public virtual void PuzzleSolved()
    {
        isSolved = true;
        TriggerDialogue();
    }

    public virtual void PuzzleSolvedReminder() { }
}
