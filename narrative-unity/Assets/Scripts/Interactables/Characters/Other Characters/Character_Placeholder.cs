using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Placeholder : CharacterWithDialogue
{
    public ItemStats requiredItemKey;

    public override void Use()
    {
        TriggerItemDialogue(GameManager.GLOBAL.inventoryManager.GetCurrentItem());
    }

    public override bool TriggerItemDialogue(Item item = null)
    {
        if (!item)
            return false;

        if (item.itemStats == requiredItemKey)
            item.UseItem();

        return base.TriggerItemDialogue(item);
    }
}
