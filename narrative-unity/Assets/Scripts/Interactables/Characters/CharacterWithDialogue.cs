using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Character))]
public class CharacterWithDialogue : Interactable
{
    Color characterGlowColorOverride = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .2f);

    protected override void StartFunctions()
    {
        base.StartFunctions();

        OverrideGlowColor(characterGlowColorOverride);
    }

    /*
     * Override UpdateFunctions to prevent glow lerping
     */
    protected override void UpdateFunctions()
    {

    }
}
