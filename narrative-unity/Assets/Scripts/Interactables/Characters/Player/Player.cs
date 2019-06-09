using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(Flowchart))]
public class Player : CharacterWithDialogue
{
    public Camera thirdPersonCamera;

    public void WrongItemDialogue()
    {
        if (IsInDialogueCheck())
            return;

        // TODO: Add randomization
        Block wrongItemBlock = flowchart.FindBlock("WrongItem");
        flowchart.ExecuteBlock(wrongItemBlock);
    }
}
