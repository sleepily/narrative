using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    protected MeshRenderer meshRenderer;
    protected Color initialColor;

    protected bool focused = false;

    [SerializeField]
    bool isUsable = true;
    
    private void OnEnable()
    {
        EventManager.Global.StartListening("Item_Key", EventFunction);
    }

    private void OnDisable()
    {
        EventManager.Global.StopListening("Item_Key", EventFunction);
    }

    public void EventFunction(string parameter = "")
    {
        switch (parameter)
        {
            case "interact":
                PickupItem();
                break;
            case "search":
                break;
            case "focus":
                FocusItem();
                break;
            case "unfocus":
                UnfocusItem();
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        initialColor = meshRenderer.material.color;
    }

    public void FocusItem()
    {
        //TODO: use overlay/outline instead of color change
        meshRenderer.material.color = Color.green;
    }

    void UnfocusItem()
    {
        meshRenderer.material.color = initialColor;
    }

    public void PickupItem()
    {
        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name); // tag + "_" + name

        gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (!isUsable)
            return;

        EventManager.Global.TriggerEvent("Inventory_Remove", gameObject, name); // tag + "_" + name
    }
}
