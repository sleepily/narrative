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
     * Execute Start block, which will handle conditions and jumps
     */
    public virtual void TriggerDialogue()
    {
        if (IsInDialogueCheck())
            return;

        GameManager.GLOBAL.dialogue.QueueForRead(flowchart);
        return;
    }
    /*
     * Trigger a specified block instead of Start
     */
    public virtual void TriggerDialogue(string blockID)
    {
        if (IsInDialogueCheck())
            return;

        if (!flowchart.HasBlock(blockID))
            return;

        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, blockID);
        return;
    }

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
    public override void Interact() => TriggerDialogue();

    public override void Use() => TriggerItemDialogue();
}