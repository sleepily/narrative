using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [Header("General")]
    [SerializeField]
    protected bool isUsable = true;
    protected bool isInInventory = false;

    [Header("Item inspection")]
    float rotationSpeed = 14f;
    float scrollSpeed = 24f;
    float zoomDistance = 2f;

    /*
     * Pick up item when clicked.
     */
    public override void Interact()
    {
        base.Interact();

        PickupItem();
    }

    /*
     * Don't trigger dialogue when using another item on this item
     */
    public override void Use() { }

    public void PickupItem()
    {
        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name);
        isInInventory = true;

        SetGlowColor(Color.clear, true);

        gameObject.SetActive(false);

        GameManager.GLOBAL.inventoryManager.ToggleInventory();

        TriggerDialogue();
    }

    public void UseItem()
    {
        if (!isUsable)
            return;

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
        Quaternion resetRotation = transform.localRotation;
        resetRotation.eulerAngles = Vector3.zero;
        transform.localRotation = resetRotation;

        transform.localPosition = Vector3.zero;
    }

    public void ShowInInventory()
    {
        if (!isInInventory)
            return;

        gameObject.SetActive(GameManager.GLOBAL.inventoryManager.isOpen);
    }

    void InspectWithMouse()
    {
        // Item is not present
        if (!isInInventory)
            return;

        // Inventory isn't open
        if (!GameManager.GLOBAL.inventoryManager.isOpen)
            return;

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
