using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField]
    protected bool isUsable = true;

    public override void Interact()
    {
        base.Interact();

        PickupItem();
    }

    public override void Use()
    {
        base.Use();

        UseItem();
    }

    void PickupItem()
    {
        gameObject.SetActive(false);

        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name);

        SetGlowColor(Color.clear, true);
    }

    void UseItem()
    {
        if (!isUsable)
            return;

        EventManager.Global.TriggerEvent("Inventory_Remove", gameObject, name);
    }
}
