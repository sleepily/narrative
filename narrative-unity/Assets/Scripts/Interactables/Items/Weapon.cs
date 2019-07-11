using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    private void Update()
    {
        CheckWeaponAnimation();
    }

    void CheckWeaponAnimation()
    {
        if (Input.GetKeyDown(KeyCode.F))
            if (isCurrentItem)
                GameManager.GLOBAL.player.ItemAnimation(itemStats);
    }
}
