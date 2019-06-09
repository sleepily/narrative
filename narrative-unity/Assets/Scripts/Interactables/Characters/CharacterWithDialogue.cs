using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Character))]
public class CharacterWithDialogue : Interactable
{
    Color characterGlowColorOverride = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .2f);

    private void Start()
    {

    }

    protected override void StartFunctions()
    {
        base.StartFunctions();

        OverrideGlowColor(characterGlowColorOverride);
    }

    private void OnEnable()
    {
        EventManager.Global.StartListening(name, EventFunction);
    }

    private void Update()
    {
        IsInDialogueCheck();
    }
}
