using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DialogueTrigger))]
public class Character : Interactable
{
    DialogueTrigger dialogueTrigger;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnEnable()
    {
        EventManager.Global.StartListening(name, EventFunction);
    }

    public void EventFunction(string parameter = "")
    {
        switch (parameter)
        {
            case "interact":
                dialogueTrigger.TriggerDialogue();
                break;
            case "focus":
                //TODO: small pop-up
                break;
            case "unfocus":
                //TODO: remove small pop-up
                break;
            default:
                break;
        }
    }
}
