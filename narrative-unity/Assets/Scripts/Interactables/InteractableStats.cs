using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Interactable", menuName = "Game Mechanics/Interactables/Interactable")]
public class InteractableStats : ScriptableObject
{
    public bool hasGlow = true;
    public Color glowColor = Color.yellow;

    public List<ItemStats> itemsToInteractWith = new List<ItemStats>();

    public virtual void Focus() { }

    public virtual void Unfocus() { }

    public virtual void Interact() { }

    public virtual void Use() { }
}
