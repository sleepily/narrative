using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Fungus;

public class InventoryManager : MonoBehaviour
{
    public bool isOpen = false;

    Item currentItem;
    public Transform itemViewParent;
    public TextMeshProUGUI currentItemText;
    public SayDialog itemSayDialog;

    public List<GameObject> objectsToHide;

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
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsEmpty)
            {
                Debug.Log(stringInventoryEmpty);
                return;
            }

            currentItem = items.First().Value;

            Debug.Log(string.Format(stringItemSelected, currentItem.name));
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        CursorLock.SetCursorLock(!isOpen);

        HideObjects();

        if (GetCurrentItem())
        {
            currentItem.transform.parent = itemViewParent;
            currentItem.ResetTransform();
            currentItem.ShowInInventory();
        }
    }

    void HideObjects()
    {
        foreach (GameObject gameObject in objectsToHide)
        {
            gameObject.SetActive(!isOpen);
        }
    }

    bool IsEmpty => (items == null || items.Count == 0);

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
        item.gameObject.SetActive(true);
        item.transform.parent = itemViewParent;
        item.transform.localPosition = Vector3.forward;
    }

    public Item GetCurrentItem() => currentItem;
}
