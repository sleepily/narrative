using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [Header("General")]
    public ItemStats itemStats;

    protected bool isInInventory = false;
    public bool isCurrentItem = false;

    [Header("Item inspection")]
    float rotationSpeed = 14f;
    float scrollSpeed = 24f;
    float zoomDistance = 1.2f;

    /*
     * Pick up item when clicked.
     */
    public override void Interact()
    {
        if (isInInventory)
            return;

        base.Interact();

        PickupItem();
    }

    /*
     * Don't trigger dialogue when using another item on this item
     */
    public override void Use()
    {
        if (isInInventory)
            return;
    }

    public void PickupItem()
    {
        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name);
        isInInventory = true;

        SetGlowColor(Color.clear, true);

        GameManager.GLOBAL.inventoryManager.ToggleInventory();

        TriggerDialogue();
    }

    public void UseItem()
    {
        EventManager.Global.TriggerEvent("Inventory_Remove", gameObject, name);
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
        if (!GameManager.GLOBAL.inventoryManager.isOpen)
            return;

        // Prevent mouse interaction advancing dialogue
        if (IsInDialogueCheck())
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

        localPosition.z = Mathf.Clamp(localPosition.z, -zoomDistance, zoomDistance);
        transform.localPosition = localPosition;
    }
}
