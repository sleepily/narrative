using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    protected MeshRenderer meshRenderer;
    protected Color glowColor = Color.yellow;
    protected Color currentGlowColor, desiredGlowColor;
    
    protected float colorLerpFactor = .2f;
    protected bool lerpIsFinished = true;

    [SerializeField]
    protected bool isUsable = true;

    private void OnEnable()
    {
        EventManager.Global.StartListening(name, ItemEventFunction);
    }

    private void OnDisable()
    {
        EventManager.Global.StopListening(name, ItemEventFunction);
    }

    public virtual void ItemEventFunction(GameObject sender, string parameter = "")
    {
        switch (parameter)
        {
            case "interact":
                PickupItem();
                break;
            case "use":
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

    void SetGlowColor(Color color, bool isInstant = false)
    {
        desiredGlowColor = color;
        lerpIsFinished = isInstant;

        if (!isInstant)
            return;

        currentGlowColor = desiredGlowColor;
        meshRenderer.material.SetColor("_GlowColor", desiredGlowColor);
    }

    public void PickupItem()
    {
        gameObject.SetActive(false);

        EventManager.Global.TriggerEvent("Inventory_Add", gameObject, name); // tag + "_" + name

        SetGlowColor(Color.clear, true);
    }

    public void UseItem()
    {
        if (!isUsable)
        {
            return;
        }

        EventManager.Global.TriggerEvent("Inventory_Remove", gameObject, name); // tag + "_" + name
    }
}
