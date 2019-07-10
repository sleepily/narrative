using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class CharacterWithDialogue : InteractableWithDialogue
{
    Color characterGlowColorOverride = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .2f);

    bool isDead = false;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        OverrideGlowColor(characterGlowColorOverride);
    }

    /*
     * Override UpdateFunctions to prevent glow lerping
     */
    protected override void UpdateFunctions() { }

    public void SetMenuInProgress(bool inProgress) =>
        GameManager.GLOBAL.dialogue.SetMenuInProgress(inProgress);

    public override void Interact()
    {
        if (isDead)
            return;

        base.Interact();
    }

    public override void Use()
    {
        if (isDead)
            return;

        base.Use();
    }

    public void Die() => isDead = true;
}
