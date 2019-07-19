using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEvent : MonoBehaviour
{
    public enum TriggerEvent { OnEnable, Awake, Start, Update, OnDisable, OnDestroy }
    public TriggerEvent triggerEvent = TriggerEvent.Start;

    public UnityEvent unityEvent;

    private void Awake()
    {
        if (triggerEvent == TriggerEvent.Awake)
            unityEvent.Invoke();
    }

    private void Start()
    {
        if (triggerEvent == TriggerEvent.Start)
            unityEvent.Invoke();
    }

    private void OnEnable()
    {
        if (triggerEvent == TriggerEvent.OnEnable)
            unityEvent.Invoke();
    }

    private void OnDisable()
    {
        if (triggerEvent == TriggerEvent.OnDisable)
            unityEvent.Invoke();
    }

    private void OnDestroy()
    {
        if (triggerEvent == TriggerEvent.OnDestroy)
            unityEvent.Invoke();
    }

    private void Update()
    {
        if (triggerEvent == TriggerEvent.Update)
            unityEvent.Invoke();
    }
}
