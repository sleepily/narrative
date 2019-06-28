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

    public void Lock()
    {
        isLocked = true;
        playerMovement.StopMoving();
    }

    public void Unlock() => isLocked = false;
}
