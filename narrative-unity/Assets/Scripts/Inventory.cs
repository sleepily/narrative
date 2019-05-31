using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, Item> items;
    
    private void OnEnable()
    {
        EventManager.Global.StartListening("Inventory_Add", Add);
        EventManager.Global.StartListening("Inventory_Remove", Remove);
    }

    private void Start()
    {
        items = new Dictionary<string, Item>();
    }

    public void Add(string parameter = "")
    {
        Item itemToAdd = EventManager.Global.lastSender.GetComponent<Item>();

        items.Add(parameter, itemToAdd);
    }

    public void Remove(string parameter = "")
    {
        items.Remove(parameter);
    }
}
