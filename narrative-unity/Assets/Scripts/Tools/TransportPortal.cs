using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TransportPortal : MonoBehaviour
{
    public TeleportLocation destination;
    public bool isEnabled = true;

    public void SetEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }
}
