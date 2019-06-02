using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    public void TriggerDialogue()
    {
        GameManager.GLOBAL.dialogueManager.StartDialogue(dialogue, interactable);
    }

    public void TriggerItemDialogue()
    {
        Item dialogueItem = GameManager.GLOBAL.inventoryManager.GetCurrentItem();
        GameManager.GLOBAL.dialogueManager.StartDialogue(dialogue, interactable);
    }
}
