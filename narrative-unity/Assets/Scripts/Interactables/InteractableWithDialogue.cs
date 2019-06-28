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

        flowchart.ExecuteBlock("Start");
        GameManager.GLOBAL.dialogue.WaitForPlayerRead(flowchart);
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
            item = GameManager.GLOBAL.inventory.GetCurrentItem();

        if (!item)
        {
            Debug.Log(string.Format("<color=orange>{0}:</color> No item for dialogue.", name));
            return false;
        }

        Block itemBlock = flowchart.FindBlock(item.itemStats.ID);

        if (!itemBlock)
        {
            Block wrongItemBlock = flowchart.FindBlock("WrongItem");

            if (wrongItemBlock)
            {
                flowchart.ExecuteBlock(wrongItemBlock);
                GameManager.GLOBAL.dialogue.WaitForPlayerRead(flowchart);
            }
            else
            {
                GameManager.GLOBAL.player.WrongItemDialogue();
                GameManager.GLOBAL.dialogue.WaitForPlayerRead(GameManager.GLOBAL.player.flowchart);
            }

            return false;
        }

        flowchart.ExecuteBlock(itemBlock);
        GameManager.GLOBAL.dialogue.WaitForPlayerRead(flowchart);
        return true;
    }
    public override void Interact() => TriggerDialogue();

    public override void Use() => TriggerItemDialogue();
}