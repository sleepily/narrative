using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Fungus.Character))]
[RequireComponent(typeof(Flowchart))]
public class CharacterWithDialogue : Interactable
{
    Flowchart flowchart;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        flowchart = GetComponent<Flowchart>();
    }

    private void OnEnable()
    {
        EventManager.Global.StartListening(name, EventFunction);
    }

    public void EventFunction(GameObject sender, string parameter = "")
    {
        switch (parameter)
        {
            case "interact":
                TriggerDialogue();
                break;
            case "use":
                TriggerItemDialogue();
                break;
            case "focus":
                //TODO: small pop-up
                break;
            case "unfocus":
                //TODO: remove small pop-up
                break;
            default:
                break;
        }
    }

    /*
     * Execute Start block, which will handle conditions and jumps
     */
    public bool TriggerDialogue()
    {
        Block startBlock = flowchart.FindBlock("Start");
        return flowchart.ExecuteBlock(startBlock);
    }

    /*
     * Trigger character dialogue using an item and jump to the block with the item's ID
     */
    public bool TriggerItemDialogue(Item item = null)
    {
        if (!item)
            item = GameManager.GLOBAL.inventoryManager.GetCurrentItem();

        if (!item)
        {
            Debug.Log(string.Format("<color=orange>{0}:</color> No item for dialogue.", name));
            return false;
        }

        Block itemBlock = flowchart.FindBlock(item.name);

        if (!itemBlock)
        {
            Debug.Log(string.Format("<color=orange>{0}:</color> Cannot trigger dialogue with {1}.", name, item.name));
            return false;
        }

        flowchart.ExecuteBlock(itemBlock);
        return true;
    }
}
