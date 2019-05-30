using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    protected EventManager eventManager;

    private void Awake()
    {
        eventManager = EventManager.GlobalManager;
    }

    // start listening on enable
    private void OnEnable() { }

    // stop listening on disable
    private void OnDisable() { }
}
