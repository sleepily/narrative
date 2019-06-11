using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Placeholder : CharacterWithDialogue
{
    public override void Use()
    {
        TriggerItemDialogue(GameManager.GLOBAL.inventoryManager.GetCurrentItem());
    }

    public override bool TriggerItemDialogue(Item item = null)
    {
        if (!item)
            return false;

        if (item.GetType() == typeof(Item_GoldenKey))
            item.UseItem();

        return base.TriggerItemDialogue(item);
    }
}
