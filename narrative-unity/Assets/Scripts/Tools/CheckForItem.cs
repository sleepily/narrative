using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckForItem : MonoBehaviour
{
    public ItemStats itemToCheckFor;
    public UnityEvent invokeOnTrigger;

    [SerializeField]
    bool onlyTriggerOnce = true;
    bool triggered = false;

    public void Check()
    {
        if (triggered)
            if (onlyTriggerOnce)
                return;

        if (!GameManager.GLOBAL.inventory.GetItemInInventory(itemToCheckFor.ID, out Item outItem))
            return;

        InvokeEvent();
    }

    void InvokeEvent()
    {
        triggered = true;
        invokeOnTrigger.Invoke();
    }
}
