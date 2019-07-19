using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

[RequireComponent(typeof(CharacterController))]
public class Player : InteractableWithDialogue
{
    public Camera thirdPersonCamera;
    public TeleportPlayer teleportPlayer;
    PlayerMovement playerMovement;
    Animator animator;
    public ItemStats[] weapons;

    public Image hudItemImage;
    public Transform itemHold, itemInspection;

    public GameObject cursor;

    public bool hasLockedMovement { get; private set; } = false;

    protected override void GetAllComponents()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
        GameManager.GLOBAL.inventory.hudItemImage = hudItemImage;
        GameManager.GLOBAL.inventory.itemHotCornerParent = itemHold;
        GameManager.GLOBAL.inventory.itemInspectionParent = itemInspection;
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

    public void PickupAndUseItemAnimation() => animator.SetTrigger("UseItem");

    public void ItemAnimation(ItemStats item)
    {
        if (!item.isWeapon)
        {
            PickupAndUseItemAnimation();
            return;
        }

        if (weapons.Length == 0)
            return;

        foreach (ItemStats weapon in weapons)
            if (weapon == item)
                animator.SetTrigger(item.ID);

        StartCoroutine(LockPlayerForSeconds(item.weaponCooldownTime));
    }

    IEnumerator LockPlayerForSeconds(float seconds)
    {
        seconds = Mathf.Abs(seconds);

        LockMovement();

        yield return new WaitForSeconds(seconds);

        if (!GameManager.GLOBAL.dialogue.dialogueInProgress)
            UnlockMovement();
    }

    public void HideCursor(bool hide)
    {
        cursor.SetActive(!hide);
    }
}
