using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    [HideInInspector]
    public Interactable interactable;

    [TextArea(1, 30)]
    public string[] phrases;
}
