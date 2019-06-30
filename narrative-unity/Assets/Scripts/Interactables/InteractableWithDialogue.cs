using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class InteractableWithDialogue : Interactable
{
    [Header("Dialogue")]
    protected Flowchart flowchart;

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        GetFlowchart();
    }

    protected Flowchart GetFlowchart()
    {
        if (!flowchart)
            flowchart = GetComponent<Flowchart>();

        return flowchart;
    }

    protected bool IsInDialogueCheck()
    {
        if (!GetFlowchart())
            return false;

        /*
        if (GameManager.GLOBAL.player.IsLocked())
            return true;
        */

        return flowchart.HasExecutingBlocks();
    }

    /*
     * Queue executing Start block, which will handle conditions and jumps
     */
    public virtual void TriggerDialogue() =>
        GameManager.GLOBAL.dialogue.QueueForRead(flowchart);

    /*
     * Queue triggering a specified block
     */
    public virtual void TriggerDialogue(string blockID) =>
        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, blockID);

    /*
     * Trigger character dialogue at block of itemID
     */
    public virtual bool TriggerItemDialogue(Item item = null)
    {
        if (IsInDialogueCheck())
            return false;

        if (!item)
            item = GameManager.GLOBAL.inventory.GetCurrentItem();

        if (!item)
        {
            Debug.Log($"<color=orange>{name}:</color> No item for dialogue.");
            return false;
        }

        Block itemBlock = flowchart.FindBlock(item.itemStats.ID);

        if (!itemBlock)
        {
            Block wrongItemBlock = flowchart.FindBlock("WrongItem");

            if (wrongItemBlock)
                GameManager.GLOBAL.dialogue.QueueForRead(flowchart, wrongItemBlock);
            else
                GameManager.GLOBAL.player.WrongItemDialogue();

            return false;
        }

        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, itemBlock);
        return true;
    }
    public override void Interact()
    {
        if (!GameManager.GLOBAL.dialogue.dialogueInProgress)
            TriggerDialogue();
    }

    public override void Use() => TriggerItemDialogue();
}