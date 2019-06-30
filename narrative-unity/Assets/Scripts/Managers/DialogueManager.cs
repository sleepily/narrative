using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueManager : MonoBehaviour
{
    struct DialoguePair
    {
        public Flowchart flowchart;
        public Block block;

        public DialoguePair(Flowchart flowchart, string blockID)
        {
            if (blockID == null)
                blockID = "Start";

            this.flowchart = flowchart;
            this.block = flowchart.FindBlock(blockID);
        }

        public DialoguePair(Flowchart flowchart, Block block)
        {
            if (block == null)
                this.block = flowchart.FindBlock("Start");

            this.flowchart = flowchart;
            this.block = block;
        }
    }

    public bool dialogueInProgress { get; private set; } = false;
    Queue<DialoguePair> queue = new Queue<DialoguePair>();

    /*
     * Enqueue a dialogue pair to be executed
     */
    public void QueueForRead(Flowchart flowchart, string blockID = null) =>
        Enqueue(new DialoguePair(flowchart, blockID));

    public void QueueForRead(Flowchart flowchart, Block block) =>
        Enqueue(new DialoguePair(flowchart, block));

    void Enqueue(DialoguePair pair)
    {
        queue.Enqueue(pair);

        Debug.Log($"Enqueued Dialogue from {pair.flowchart.gameObject.name}");

        if (!dialogueInProgress)
            ExecuteNextDialoguePair();
    }

    bool ExecuteNextDialoguePair()
    {
        if (queue.Count == 0)
            return false;

        DialoguePair dialoguePair = queue.Dequeue();
        
        dialoguePair.flowchart.ExecuteBlock(dialoguePair.block);
        WaitForDialogue(dialoguePair.flowchart);
        return true;
    }

    void WaitForDialogue(Flowchart flowchart)
    {
        dialogueInProgress = true;
        GameManager.GLOBAL.player.SetMovementLock(true);

        StartCoroutine(Coroutine_WaitForDialogue(flowchart));
    }

    IEnumerator Coroutine_WaitForDialogue(Flowchart flowchart)
    {
        // Wait for the player to read all dialogue
        while (flowchart.HasExecutingBlocks())
            yield return null;

        dialogueInProgress = false;


        // Keep the chain of queued up dialogue pairs running
        // Unlock the player if the queue's final flowchart belongs to a puzzle
        if (!ExecuteNextDialoguePair())
            if (CheckForPuzzle(flowchart))
                GameManager.GLOBAL.player.SetMovementLock(false);
    }

    /*
     * Fix player not being unlocked after trying to solve a puzzle
     */
    bool CheckForPuzzle(Flowchart flowchart) =>
        flowchart.gameObject.GetComponent<Puzzle>() ?? false;
}
