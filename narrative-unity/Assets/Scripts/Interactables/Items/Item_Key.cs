using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Key : Item
{
    MeshRenderer meshRenderer;
    Color initialColor;

    bool focused = false;

    private void Start()
    {
        GetAllComponents();
    }

    void GetAllComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        initialColor = meshRenderer.material.color;
    }

    private void OnEnable()
    {
        EventManager.GlobalManager.StartListening("Item_Key", EventListener);
    }

    private void OnDisable()
    {
        EventManager.GlobalManager.StopListening("Item_Key", EventListener);
    }

    private void Update()
    {
        ResetColor();
    }

    void ResetColor()
    {
        if (focused)
        {
            meshRenderer.material.color = initialColor;
            focused = false;
        }
    }

    public void EventListener(string parameter = "")
    {
        switch (parameter)
        {
            case "use":
                gameObject.SetActive(false);
                break;
            case "focus":
                //TODO: use overlay/outline instead of color change
                meshRenderer.material.color = Color.green;
                focused = true;
                break;
            default:
                break;
        }
    }
}
