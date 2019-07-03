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
            if (blockID == null || string.IsNullOrWhiteSpace(blockID))
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
    public bool menuInProgress { get; private set; } = false;

    Queue<DialoguePair> queue = new Queue<DialoguePair>();

    bool logVerbose = true;

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

        if (logVerbose)
            Debug.Log($"Enqueued Dialogue from {pair.flowchart.gameObject.name}");

        if (!dialogueInProgress)
            ExecuteNextDialoguePair();
    }

    bool ExecuteNextDialoguePair()
    {
        if (queue.Count == 0)
            return false;

        DialoguePair dialoguePair = queue.Dequeue();

        if (logVerbose)
            Debug.Log($"Trying to execute {dialoguePair.flowchart.gameObject.name}:{dialoguePair.block.BlockName}");

        if (!dialoguePair.flowchart.ExecuteIfHasBlock(dialoguePair.block.BlockName))
        {
            Debug.Log("Cannot execute block. Skipping.");
            return ExecuteNextDialoguePair();
        }

        if (logVerbose)
            Debug.Log($"Executing {dialoguePair.flowchart.gameObject.name}:{dialoguePair.block.BlockName}...");

        SpecialChecksBefore(dialoguePair.flowchart);
        WaitForDialogue(dialoguePair.flowchart);
        return true;
    }

    public void SetMenuInProgress(bool inProgress)
    {
        menuInProgress = inProgress;

        // Unlock the cursor when the player has to make a choice
        if (menuInProgress)
            StartCoroutine(CheckForMenuOver());
    }

    IEnumerator CheckForMenuOver()
    {
        // Wait for some time to prevent weird things
        yield return new WaitForSeconds(.6f);

        // A choice menu is presented to the player
        CursorLock.SetCursorLock(false);

        // Wait until the choice block is activated
        while (!dialogueInProgress)
            yield return null;

        // Lock the player again
        menuInProgress = false;
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

        if (logVerbose)
            Debug.Log($"Finished Flowchart on {flowchart.gameObject.name}.");

        dialogueInProgress = false;

        SpecialChecksAfter(flowchart);

        // Keep the chain of queued up dialogue pairs running
        // Execute final special checks if this is the last queued flowchart
        if (!ExecuteNextDialoguePair())
            SpecialChecksFinal(flowchart);
    }

    void SpecialChecksBefore(Flowchart flowchart)
    {
        flowchart.gameObject.GetComponent<Cellphone>()?.EnablePhoneScreen(true);
    }

    void SpecialChecksAfter(Flowchart flowchart)
    {
        flowchart.gameObject.GetComponent<Cellphone>()?.EnablePhoneScreen(false);
    }

    void SpecialChecksFinal(Flowchart flowchart)
    {
        // Unlock the player if the queue's final flowchart did not belong to an item
        if (!flowchart.gameObject.GetComponent<Item>())
            GameManager.GLOBAL.player.SetMovementLock(false);

        if (flowchart.gameObject.GetComponent<Cellphone>())
            GameManager.GLOBAL.player.SetMovementLock(false);
    }
}
