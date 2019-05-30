using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static EventManager globalManager;
    public static EventManager GlobalManager
    {
        get
        {
            if (globalManager == null)
                globalManager = new EventManager();

            return globalManager;
        }
    }

    public delegate void EventFunction(string param = "");

    public Dictionary<string, List<EventFunction>> events;
    public Dictionary<string, List<UnityEvent>> alternateEvents;

    public void StartListening(string eventID, EventFunction listener)
    {
        // create events list if it doesn't exist
        if (events == null)
            events = new Dictionary<string, List<EventFunction>>();

        // create event key if it doesn't exist
        if (!events.ContainsKey(eventID))
            events.Add(eventID, new List<EventFunction>());

        // add listener calling this function
        events[eventID].Add(listener);
    }

    public void StopListening(string eventID, EventFunction listener)
    {
        // exit if dictionary doesn't exist
        if (events == null)
            return;

        // exit if key doesn't exist
        if (!events.ContainsKey(eventID))
            return;

        // remove this listener from the event
        events[eventID].Remove(listener);
    }

    public void TriggerEvent(string eventID, string param = "")
    {
        // exit if dictionary doesn't exist
        if (events == null)
            return;
        
        if (!events.ContainsKey(eventID))
        {
            Debug.Log(string.Format("EventID {0} is unknown.", eventID));
            return;
        }

        for (int listenerIndex = events[eventID].Count - 1; listenerIndex >= 0; listenerIndex--)
        {
            events[eventID][listenerIndex](param);
        }
    }
}
