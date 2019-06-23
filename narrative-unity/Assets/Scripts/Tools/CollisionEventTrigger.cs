using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEventTrigger : MonoBehaviour
{
    public bool onlyOnce = true;
    public UnityEvent invokeOnTrigger;
    int triggerCounter = 0;

    private void OnTriggerEnter(Collider collider)
    {
        CheckTrigger(collider);
    }

    void CheckTrigger(Collider collider)
    {
        Player player = collider.GetComponent<Player>();

        Debug.Log("Checking for player");
        if (!player)
            return;

        Debug.Log("Checking trigger count");
        if (onlyOnce && triggerCounter > 0)
            return;

        Debug.Log("Invoke trigger");
        triggerCounter++;
        invokeOnTrigger.Invoke();
    }
}
