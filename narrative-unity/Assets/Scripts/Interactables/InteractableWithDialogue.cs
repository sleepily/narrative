using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class InteractableWithDialogue : Interactable
{
    [Header("Dialogue")]
    protected Flowchart flowchart;
    protected bool isInDialogue;

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

        return isInDialogue = flowchart.HasExecutingBlocks();
    }

    /*
     * Execute Start block, which will handle conditions and jumps
     */
    public virtual void TriggerDialogue()
    {
        if (IsInDialogueCheck())
            return;

        flowchart.ExecuteBlock("Start");
        return;
    }

    /*
     * Trigger character dialogue using an item and jump to the block with the item's ID
     */
    public virtual bool TriggerItemDialogue(Item item = null)
    {
        if (IsInDialogueCheck())
            return false;

        if (!item)
            item = GameManager.GLOBAL.inventoryManager.GetCurrentItem();

        if (!item)
        {
            Debug.Log(string.Format("<color=orange>{0}:</color> No item for dialogue.", name));
            return false;
        }

        Block itemBlock = flowchart.FindBlock(item.itemStats.name);

        if (!itemBlock)
        {
            Block wrongItemBlock = flowchart.FindBlock("WrongItem");

            if (wrongItemBlock)
                flowchart.ExecuteBlock(wrongItemBlock);
            else
                GameManager.GLOBAL.player.WrongItemDialogue();

            return false;
        }

        flowchart.ExecuteBlock(itemBlock);
        return true;
    }
    public override void Interact() => TriggerDialogue();

    public override void Use() => TriggerItemDialogue();
}