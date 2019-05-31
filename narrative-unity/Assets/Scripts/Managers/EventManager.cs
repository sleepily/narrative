using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static EventManager global;
    public static EventManager Global
    {
        get
        {
            if (global == null)
                global = new EventManager();

            return global;
        }
    }

    public delegate void EventFunction(string param = "");

    public Dictionary<string, List<EventFunction>> events;
    public Dictionary<string, List<UnityEvent>> alternateEvents;

    public GameObject lastSender;

    public void StartListening(string eventID, EventFunction eventFunction)
    {
        string log;

        // create events list if it doesn't exist
        if (events == null)
            events = new Dictionary<string, List<EventFunction>>();

        // create event key if it doesn't exist
        if (!events.ContainsKey(eventID))
            events.Add(eventID, new List<EventFunction>());

        // add listener calling this function
        events[eventID].Add(eventFunction);

        log = string.Format("<color={1}>EventManager:</color> Listening for {0}.", eventID, "lime");
        Debug.Log(log);
    }

    public void StopListening(string eventID, EventFunction listener)
    {
        string log;

        // exit if dictionary doesn't exist
        if (events == null)
            return;

        // exit if key doesn't exist
        if (!events.ContainsKey(eventID))
            return;

        // remove this listener from the event
        events[eventID].Remove(listener);

        log = string.Format("<color={1}>EventManager:</color> Stop listening for {0}.", eventID, "orange");
        Debug.Log(log);
    }

    public void TriggerEvent(string eventID, GameObject sender, string parameter = "")
    {
        string log;

        // exit if dictionary doesn't exist
        if (events == null)
            return;
        
        if (!events.ContainsKey(eventID))
        {
            log = string.Format("<color={1}>EventManager:</color> EventID {0} does not exist.", eventID, "red");
            Debug.Log(log);

            return;
        }

        log = string.Format("<color={2}>EventManager:</color> Triggering {0} with \"{1}\".", eventID, parameter, "cyan");
        Debug.Log(log);

        lastSender = sender;

        for (int listenerIndex = events[eventID].Count - 1; listenerIndex >= 0; listenerIndex--)
        {
            events[eventID][listenerIndex](parameter);
        }
    }
}
