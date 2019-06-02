using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    Item currentItem;

    public TextMeshProUGUI currentItemText;

    public Dictionary<string, Item> items;

    string stringItemAdded = "<color=lime>InventoryManager:</color> Added item {0}.";
    string stringItemRemoved = "<color=cyan>InventoryManager:</color> Removed item {0}.";
    string stringItemSelected = "<color=cyan>InventoryManager:</color> Currently selected Item is {0}.";
    string stringItemNotFound = "<color=cyan>InventoryManager:</color> Couldn't find item {0}.";
    string stringInventoryEmpty = "<color=orange>InventoryManager:</color> Inventory is empty.";
    string stringItemAlreadyInInventory = "<color=orange>InventoryManager:</color> Item {0} already in inventory.";

    private void OnEnable()
    {
        EventManager.Global.StartListening("Inventory_Add", Add);
        EventManager.Global.StartListening("Inventory_Remove", Remove);
    }

    private void Start()
    {
        items = new Dictionary<string, Item>();
    }

    public void Add(GameObject sender, string parameter = "")
    {
        Item itemToAdd = sender.GetComponent<Item>();

        if (items.ContainsKey(parameter))
        {
            Debug.Log(string.Format(stringItemAlreadyInInventory, itemToAdd.name));
            return;
        }

        items.Add(parameter, itemToAdd);

        SetCurrentItem(itemToAdd);

        Debug.Log(string.Format(stringItemAdded, itemToAdd.name));
    }

    public void Remove(GameObject sender, string parameter = "")
    {
        items.Remove(parameter);

        Debug.Log(string.Format(stringItemRemoved, parameter));
    }

    private void Update()
    {
        GetInput(); //TODO: replace with HUD
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsEmpty())
            {
                Debug.Log(stringInventoryEmpty);
                return;
            }

            currentItem = items.First().Value;

            Debug.Log(string.Format(stringItemSelected, currentItem.name));
        }
    }

    bool IsEmpty()
    {
        return (items == null || items.Count == 0);
    }

    bool SetCurrentItem(string itemID)
    {
        Item foundItem;
        if (!items.TryGetValue(itemID, out foundItem))
        {
            Debug.Log(string.Format(stringItemNotFound, itemID));
            return false;
        }

        SetCurrentItem(foundItem);
        return true;
    }

    void SetCurrentItem(Item item)
    {
        currentItem = item;
        currentItemText.text = "Current Item: " + item.name;
    }

    public Item GetCurrentItem()
    {
        return currentItem;
    }
}
