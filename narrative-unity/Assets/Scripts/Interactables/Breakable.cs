using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Interactable
{
    public GameObject brokenObjectToSpawn;

    public override void Use()
    {
        ItemStats itemUsed = GameManager.GLOBAL.inventoryManager.GetCurrentItem().itemStats;

        if (itemUsed.isWeapon)
            return;

        Break();
    }

    public void Break() => Destroy(gameObject);
}
