using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CreateAssetMenu(fileName = "New Item", menuName = "Game Mechanics/Interactables/Item")]
public class ItemStats : ScriptableObject
{
    public new string name;
    public bool canBePickedUp = true;
    public float inventoryInspectionScale = 1f;
    public float handheldScale = 1f;
}
