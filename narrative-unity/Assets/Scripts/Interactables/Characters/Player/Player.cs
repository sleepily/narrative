using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(CharacterController))]
public class Player : CharacterWithDialogue
{
    public Camera thirdPersonCamera;
    public TeleportPlayer teleportPlayer;
    PlayerMovement playerMovement;

    [HideInInspector]
    protected bool isLocked = false;

    protected override void StartFunctions()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void GetAllComponents() => GetFlowchart();

    protected override void UpdateFunctions() { }

    public void WrongItemDialogue()
    {
        if (IsInDialogueCheck())
            return;

        // TODO: Add randomization
        Block wrongItemBlock = flowchart.FindBlock("WrongItem");
        flowchart.ExecuteBlock(wrongItemBlock);
    }

    public bool IsLocked() => isLocked;

    public void SetLocked(bool locked)
    {
        if (locked)
            Lock();
        else
            Unlock();
    }

    public void Lock()
    {
        isLocked = true;
        playerMovement.StopMoving();
        Debug.Log("Lock Player");
    }

    public void Unlock()
    {
        Debug.Log("Unlock Player");
        isLocked = false;
    }
}
