using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CreateAssetMenu(fileName = "New Item", menuName = "Game Mechanics/Interactables/Item")]
public class ItemStats : ScriptableObject
{
    public string ID;
    public bool canBePickedUp = true;
    public float inventoryInspectionScale = 1f;
    public float handheldScale = 1f;
}
