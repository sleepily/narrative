using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    public Collider colliderToDisable;

    public void Disable()
    {
        if (!colliderToDisable)
            return;

        colliderToDisable.enabled = false;
    }

    public void Reenable()
    {
        if (!colliderToDisable)
            return;

        colliderToDisable.enabled = true;
    }
}
