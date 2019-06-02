using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    protected MeshRenderer meshRenderer;
    protected Color glowColor = Color.yellow;
    protected Color currentGlowColor, desiredGlowColor;

    [Range(.01f, 1f)]
    float colorLerpFactor = .5f;
    bool lerpIsFinished = true;

    [SerializeField]
    bool isUsable = true;
    
    private void OnEnable()
    {
        EventManager.Global.StartListening("Item_Key", ItemEventFunction);
    }

    private void OnDisable()
    {
        EventManager.Global.StopListening("Item_Key", ItemEventFunction);
    }

    public void ItemEventFunction(string parameter = "")
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
        SetGlowColor(Color.clear, true);
    }

    void GetAllComponents()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        glowColor = meshRenderer.material.GetColor("_GlowColor");
    }

    void SetGlowColor(Color color, bool isInstant = false)
    {
        desiredGlowColor = color;
        lerpIsFinished = isInstant;

        if (!isInstant)
            return;

        currentGlowColor = desiredGlowColor;
        meshRenderer.material.SetColor("_GlowColor", desiredGlowColor);

    }

    private void Update()
    {
        LerpGlowColor();
    }

    void LerpGlowColor()
    {
        if (lerpIsFinished)
            return;

        currentGlowColor = Color.Lerp
        (
            currentGlowColor,
            desiredGlowColor,
            Time.deltaTime / colorLerpFactor
        );

        meshRenderer.material.SetColor("_GlowColor", currentGlowColor);

        if (currentGlowColor.Equals(desiredGlowColor))
            lerpIsFinished = true;
    }

    public void FocusItem()
    {
        SetGlowColor(glowColor);
    }

    void UnfocusItem()
    {
        SetGlowColor(Color.clear);
    }

    public void PickupItem()
    {
        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name); // tag + "_" + name

        SetGlowColor(Color.clear, true);
        gameObject.SetActive(false);
    }

    public void UseItem()
    {
        if (!isUsable)
            return;

        EventManager.Global.TriggerEvent("Inventory_Remove", gameObject, name); // tag + "_" + name
    }
}
