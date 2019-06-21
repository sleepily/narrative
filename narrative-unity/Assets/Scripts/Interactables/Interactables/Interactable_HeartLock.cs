using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_HeartLock : InteractableWithDialogue
{
    public ItemStats itemToUnlock;
    public GameObject glassDoors;

    public override void Use()
    {
        if (itemToUnlock != GameManager.GLOBAL.inventoryManager.GetCurrentItem().itemStats)
            return;

        glassDoors.SetActive(false);
    }
}
