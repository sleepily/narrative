using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    public bool enabledByDefault = false;
    public bool onlyOnce = true;
    public UnityEvent invokeOnTrigger;
    int triggerCounter = 0;

    public void Enable() => enabledByDefault = true;

    private void OnTriggerEnter(Collider collider)
    {
        if (enabledByDefault)
            CheckTrigger(collider);
    }

    void CheckTrigger(Collider collider)
    {
        // Check if the player is colliding, otherwise ignore
        Player player = collider.GetComponent<Player>();

        if (!player)
            return;

        // Check trigger conditions
        if (onlyOnce && triggerCounter > 0)
            return;

        // Invoke the event
        triggerCounter++;
        invokeOnTrigger.Invoke();
    }
}
