using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class Puzzle : MonoBehaviour
{
    public bool isSolved = false;

    protected Flowchart flowchart;

    [Tooltip("Puzzle solution. Book: BookIDs, Xylophone Keys: 1/0, Code: answer")]
    public string solution = "";

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

    public bool IsInDialogueCheck()
    {
        if (!GetFlowchart())
            return false;

        return flowchart.HasExecutingBlocks();
    }

    public void TriggerDialogue()
    {
        if (!GetFlowchart())
        {
            Debug.LogWarning(name + ": Cannot trigger dialogue, Flowchart missing.");
            return;
        }

        if (IsInDialogueCheck())
            return;

        flowchart.ExecuteBlock("Start");
    }

    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() { }

    /*
     * Puzzle specific functions for resetting, checking and solving
     */
    public virtual void PuzzleReset() { }

    public virtual bool PuzzleCheck() => false;

    public virtual void PuzzleSolved()
    {
        isSolved = true;

        flowchart.SetBooleanVariable("isSolved", true);
        TriggerDialogue();
    }

    public virtual void PuzzleSolvedReminder() { }
}
