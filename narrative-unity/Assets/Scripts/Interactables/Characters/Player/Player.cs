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
    public bool hasLockedMovement { get; private set; } = false;

    protected override void GetAllComponents()
    {
        playerMovement = GetComponent<PlayerMovement>();
        GetFlowchart();
    }

    protected override void UpdateFunctions() { }

    public void WrongItemDialogue() =>
        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, "WrongItem");

    public void SetMovementLock(bool locked)
    {
        if (locked)
            LockMovement();
        else
            UnlockMovement();
    }

    public void LockMovement()
    {
        hasLockedMovement = true;
        playerMovement.StopMoving();
    }

    public void UnlockMovement()
    {
        hasLockedMovement = false;
        CursorLock.SetCursorLock(true);
    }
}
