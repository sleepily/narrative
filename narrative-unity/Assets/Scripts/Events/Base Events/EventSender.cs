using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSender : MonoBehaviour
{
    protected EventManager eventManager;

    private void Awake()
    {
        eventManager = EventManager.GlobalManager;
    }
}
