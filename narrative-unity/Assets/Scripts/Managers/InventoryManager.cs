using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Fungus;

public class InventoryManager : MonoBehaviour
{
    [Header("Information")]
    [HideInInspector]
    public bool isOpen = false;

    Item currentItem;

    [Header("References")]
    [Tooltip("Where the current Item is moved for inspection.")]
    public Transform itemInspectionParent;
    [Tooltip("Where the current Item is moved while the Inventory is closed.")]
    public Transform itemHotCornerParent;
    Transform currentItemParent;

    [Tooltip("The SayDialog where Item text is displayed.")]
    public SayDialog itemSayDialog;

    [Space]

    [Tooltip("Objects that will be hidden when the Inventory is open.")]
    public List<GameObject> objectsToHide;

    [Space]

    [Tooltip("All Items in the Inventory.")]
    [HideInInspector]
    public List<Item> items;

    bool logVerbose = false;

    string stringItemAdded = "<color=lime>InventoryManager:</color> Added item {0}.";
    string stringItemRemoved = "<color=cyan>InventoryManager:</color> Removed item {0}.";
    string stringItemNotFound = "<color=cyan>InventoryManager:</color> Couldn't find item {0}.";
    string stringItemAlreadyInInventory = "<color=orange>InventoryManager:</color> Item {0} already in inventory.";

    private void OnEnable()
    {
        EventManager.Global.StartListening("Inventory_Add", Add);
        EventManager.Global.StartListening("Inventory_Remove", Remove);
    }

    private void Start()
    {
        items = new List<Item>();
    }

    public void Add(GameObject sender, string parameter = "")
    {
        Item itemToAdd = sender.GetComponent<Item>();

        if (items.Contains(itemToAdd))
        {
            if (logVerbose)
                Debug.Log(string.Format(stringItemAlreadyInInventory, itemToAdd.itemStats.ID));
            return;
        }

        items.Add(itemToAdd);

        SetCurrentItem(itemToAdd);

        if (logVerbose)
            Debug.Log(string.Format(stringItemAdded, itemToAdd.itemStats.ID));
    }

    public void Remove(GameObject sender, string parameter = "")
    {
        Item itemToRemove = sender.GetComponent<Item>();

        if (!itemToRemove)
        {
            Debug.Log(string.Format(stringItemNotFound, parameter));
            return;
        }

        SetCurrentItem(NextItem());

        itemToRemove.RemoveFromInventory();
        items.Remove(itemToRemove);

        if (logVerbose)
            Debug.Log(string.Format(stringItemRemoved, parameter));
    }

    bool GetItemInInventory(string itemID, out Item itemOut)
    {
        foreach (Item item in items)
            if (item.itemStats.ID == itemID)
            {
                itemOut = item;
                return true;
            }

        itemOut = null;
        return false;
    }

    public void ClearInventory()
    {
        if (IsEmpty())
            return;

        items.Clear();

        Item nullItem = null;
        SetCurrentItem(nullItem);
    }

    private void Update() => GetInput();

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.GLOBAL.dialogue.dialogueInProgress)
                return;

            ToggleInventory();
        }

        // Allow reading item text again with RMB
        if (Input.GetMouseButtonDown(1))
            ShowCurrentItemDescription();

        // Allow item switching with scroll wheel
        UseScrollDeltaToChangeCurrentItem();
    }

    /*
     * Toggle Inventory on/off.
     * Picking up items should call with forceOpen = true.
     */
    public void ToggleInventory(bool forceOpen = false)
    {
        if (!forceOpen)
            isOpen = !isOpen;
        else
            isOpen = true;

        CursorLock.SetCursorLock(!isOpen);

        GameManager.GLOBAL.player.SetMovementLock(isOpen);

        currentItemParent = isOpen ? itemInspectionParent : itemHotCornerParent;

        HideObjects();

        if (GetCurrentItem())
        {
            currentItem.transform.parent = currentItemParent;
            currentItem.ResetTransform();
            currentItem.ShowInInventory();
        }
    }

    /*
     * Hide Objects such as the cursor when opening the inventory
     */
    void HideObjects()
    {
        foreach (GameObject gameObject in objectsToHide)
            gameObject.SetActive(!isOpen);
    }

    bool ShowCurrentItemDescription()
    {
        if (!isOpen || !currentItem)
            return false;

        currentItem.TriggerDialogue();
        return true;
    }

    void UseScrollDeltaToChangeCurrentItem()
    {
        if (isOpen)
            return;

        Vector2 scrollDelta = Input.mouseScrollDelta;

        if (scrollDelta.y > float.Epsilon)
            SetCurrentItem(NextItem());

        if (scrollDelta.y < -float.Epsilon)
            SetCurrentItem(PreviousItem());
    }

    Item NextItem() => GetItemRelativeToCurrent(1);

    Item PreviousItem() => GetItemRelativeToCurrent(-1);

    Item GetItemRelativeToCurrent(int indexOffset)
    {
        // If there is no item currently selected
        if (!currentItem)
            return null;

        // Fetch current Item's index
        int currentItemIndex = items.IndexOf(currentItem);

        // Apply index offset
        int newItemIndex = currentItemIndex + indexOffset;

        // If it goes below 0, set to last index
        if (newItemIndex < 0)
            newItemIndex = items.Count - 1;

        // If it goes over list length, set to first item
        if (newItemIndex >= items.Count)
            newItemIndex = 0;

        return items[newItemIndex];
    }

    bool IsEmpty() => (items == null || items.Count == 0);

    /*
     * Set item as current Item
     */
    void SetCurrentItem(Item item)
    {
        // Unset previous Item if there is one
        if (currentItem)
        {
            currentItem.isCurrentItem = false;
            currentItem.gameObject.SetActive(false);
        }

        // Set new Item
        currentItem = item;

        if (item == null)
            return;

        currentItem.isCurrentItem = true;
        currentItem.transform.parent = currentItemParent;
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.gameObject.SetActive(true);
    }

    public Item GetCurrentItem() => currentItem;
}
