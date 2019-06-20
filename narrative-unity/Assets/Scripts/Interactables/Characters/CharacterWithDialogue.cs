using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Character))]
public class CharacterWithDialogue : InteractableWithDialogue
{
    Color characterGlowColorOverride = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .2f);

    public override void StartFunctions()
    {
        base.StartFunctions();

        if (glowable)
            glowable.OverrideGlowColor(characterGlowColorOverride);
    }

    /*
     * Override UpdateFunctions to prevent glow lerping
     */
    protected override void UpdateFunctions()
    {

    }
}
