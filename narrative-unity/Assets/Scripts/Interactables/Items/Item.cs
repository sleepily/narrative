﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableWithDialogue
{
    [Header("General")]
    public ItemStats itemStats;

    public bool isInInventory { get; protected set; } = false;
    public bool isCurrentItem = false;
    bool canBePickedUp;

    [Header("Item inspection")]
    float rotationSpeed = 14f;
    float scrollSpeed = 24f;
    float zoomDistance = 1.6f;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        canBePickedUp = itemStats.canBePickedUp;
    }

    public override void Interact()
    {
        if (isInInventory)
            return;

        PickupItem();
    }

    public override void Use()
    {
        if (isInInventory)
            return;

        base.Use();
    }

    /*
     * Overwrite the item's PU state
     */
    public void SetPU(bool canBePickedUp) => this.canBePickedUp = canBePickedUp;

    /*
     * Add the item to inventory and show its flavor text
     * TODO: Fix player locking
     */
    public void PickupItem()
    {
        if (!canBePickedUp)
            return;

        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, itemStats.ID);
        isInInventory = true;

        SetGlowColor(Color.clear, true);

        GameManager.GLOBAL.player.PickupAndUseItemAnimation();
        GameManager.GLOBAL.inventory.ToggleInventory(true);

        TriggerDialogue();
    }

    public void UseItem() => UseItem(setInactive: true);

    void UseItem(bool setInactive)
    {
        GameManager.GLOBAL.inventory.Remove(gameObject, itemStats.ID);
        // Debug.Log($"Using and removing {name}");

        GameManager.GLOBAL.player.ItemAnimation(itemStats);

        if (setInactive)
            gameObject.SetActive(false);
    }

    public void RemoveFromInventory()
    {
        isInInventory = false;
        isCurrentItem = false;
    }

    protected override void UpdateFunctions()
    {
        base.UpdateFunctions();

        ShowInInventory();
        InspectWithMouse();
    }

    public void ResetTransform()
    {
        // Set rotation 
        Quaternion localRotation = transform.localRotation;
        localRotation.eulerAngles = new Vector3(.2f, .3f, 0f);
        transform.localRotation = localRotation;

        // TODO: implement this
        float itemScale = 1f;

        if (GameManager.GLOBAL.inventory.isOpen)
        {
            if (isCurrentItem)
                itemScale = itemStats.inventoryInspectionScale;
        }

        transform.localPosition = Vector3.zero;
    }

    public void ShowInInventory()
    {
        if (!isInInventory)
            return;

        gameObject.SetActive(isCurrentItem);
    }

    /*
     * Allow rotating/zooming the item in the Inventory
     */
    void InspectWithMouse()
    {
        // Item is not focused in Inventory
        if (!isCurrentItem)
            return;

        // Inventory isn't open
        if (!GameManager.GLOBAL.inventory.isOpen)
            return;

        // Prevent mouse interaction advancing dialogue
        if (GameManager.GLOBAL.dialogue.dialogueInProgress)
            return;

        // Get mouse input
        Vector2 scrollDelta;
        scrollDelta = Input.mouseScrollDelta;

        bool mouseDown = Mathf.Abs(Input.GetAxisRaw("Interact")) > float.Epsilon;
        bool scrolling = Mathf.Abs(scrollDelta.magnitude) > float.Epsilon;

        // Return if there is no mouse input
        if (!mouseDown && !scrolling)
            return;

        // Get rotation info through mouse position delta
        Vector2 rotation;
        rotation.x = Input.GetAxisRaw("Mouse X") * rotationSpeed;
        rotation.y = Input.GetAxisRaw("Mouse Y") * rotationSpeed;

        // Apply rotation
        transform.Rotate(Vector3.up, -rotation.x);
        transform.Rotate(Vector3.right, rotation.y);

        // Clamp and apply zoom
        Vector3 localPosition = transform.localPosition;
        localPosition += Vector3.back * scrollDelta.y * (scrollSpeed / 100);

        localPosition.z = Mathf.Clamp(localPosition.z, -Mathf.Abs(zoomDistance), 0f);
        transform.localPosition = localPosition;
    }
}
