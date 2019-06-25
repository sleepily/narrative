using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    public Collider colliderToDisable;

    public void Disable()
    {
        colliderToDisable.enabled = false;
    }

    public void Reenable()
    {
        colliderToDisable.enabled = true;
    }
}
