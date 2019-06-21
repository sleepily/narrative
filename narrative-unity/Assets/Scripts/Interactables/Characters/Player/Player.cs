using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[RequireComponent(typeof(CharacterController))]
public class Player : CharacterWithDialogue
{
    public Camera thirdPersonCamera;

    protected override void StartFunctions() { }

    protected override void GetAllComponents() => GetFlowchart();

    protected override void UpdateFunctions() { }

    public void WrongItemDialogue()
    {
        if (IsInDialogueCheck())
            return;

        // TODO: Add randomization
        Block wrongItemBlock = flowchart.FindBlock("WrongItem");
        flowchart.ExecuteBlock(wrongItemBlock);
    }
}
