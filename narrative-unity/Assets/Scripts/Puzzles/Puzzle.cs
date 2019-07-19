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
        if (IsInDialogueCheck())
            return;

        GameManager.GLOBAL.dialogue.QueueForRead(flowchart);
    }

    public void TriggerDialogue(string blockID = "Start")
    {
        if (IsInDialogueCheck())
            return;

        if (!flowchart.HasBlock(blockID))
        {
            Debug.Log($"Block {blockID} doesn't exist, executing block \"Start\".");
            return;
        }

        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, blockID);
    }

    private void Update() => UpdateFunctions();

    protected virtual void UpdateFunctions() { }

    /*
     * Puzzle specific functions for resetting, checking and solving
     */
    public virtual void PuzzleReset() { }

    public virtual bool PuzzleCheck() => false;
    public virtual bool PuzzleCheck(string playerInput) => false;

    public virtual void PuzzleSolved()
    {
        isSolved = true;

        if (flowchart.HasVariable("isSolved"))
            flowchart.SetBooleanVariable("isSolved", true);

        TriggerDialogue();
    }

    public virtual void PuzzleSolvedReminder() { }
}
