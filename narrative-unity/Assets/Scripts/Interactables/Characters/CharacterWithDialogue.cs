using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Flowchart))]
public class CharacterWithDialogue : Interactable
{
    Flowchart flowchart;
    bool isInDialogue;

    Block blockStart, blockWrongItem;

    Color characterGlowColorOverride = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .2f);

    private void Start()
    {

    }

    protected override void StartFunctions()
    {
        base.StartFunctions();

        OverrideGlowColor(characterGlowColorOverride);
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        flowchart = GetComponent<Flowchart>();
        blockStart = FindBlockInFlowchart("Start");
        blockWrongItem = FindBlockInFlowchart("WrongItem");
    }

    Block FindBlockInFlowchart(string blockID)
    {
        return flowchart.FindBlock(blockID);
    }

    private void OnEnable()
    {
        EventManager.Global.StartListening(name, EventFunction);
    }

    public override void Interact()
    {
        base.Interact();
        TriggerDialogue();
    }

    public override void Use()
    {
        base.Use();
        TriggerItemDialogue();
    }

    private void Update()
    {
        IsInDialogueCheck();
    }

    bool IsInDialogueCheck()
    {
        if (!flowchart)
            return false;

        return isInDialogue = flowchart.HasExecutingBlocks();
    }

    /*
     * Execute Start block, which will handle conditions and jumps
     */
    public bool TriggerDialogue()
    {
        if (isInDialogue)
            return false;
        
        return flowchart.ExecuteBlock(blockStart);
    }

    /*
     * Trigger character dialogue using an item and jump to the block with the item's ID
     */
    public bool TriggerItemDialogue(Item item = null)
    {
        if (isInDialogue)
            return false;

        if (!item)
            item = GameManager.GLOBAL.inventoryManager.GetCurrentItem();

        if (!item)
        {
            Debug.Log(string.Format("<color=orange>{0}:</color> No item for dialogue.", name));
            return false;
        }

        Block itemBlock = flowchart.FindBlock(item.name);

        if (!itemBlock)
            return flowchart.ExecuteBlock(blockWrongItem);

        flowchart.ExecuteBlock(itemBlock);
        return true;
    }
}
