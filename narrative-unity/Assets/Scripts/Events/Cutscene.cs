﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Flowchart))]
[RequireComponent(typeof(CollisionEventTrigger))]
public class Cutscene : MonoBehaviour
{
    BoxCollider boxCollider;
    Flowchart flowchart;
    CollisionEventTrigger collisionEventTrigger;

    private void Start()
    {
        GetAllComponents();
        InitializeComponents();
    }

    void GetAllComponents()
    {
        boxCollider = GetComponent<BoxCollider>();
        flowchart = GetComponent<Flowchart>();
        collisionEventTrigger = GetComponent<CollisionEventTrigger>();
    }

    void InitializeComponents()
    {
        boxCollider.isTrigger = true;

        collisionEventTrigger.enabledByDefault = true;
        collisionEventTrigger.onlyOnce = true;
    }

    public void StartCutscene() =>
        StartCutsceneAtBlock("Start");

    public void StartCutsceneAtBlock(string blockID) =>
        GameManager.GLOBAL.dialogue.QueueForRead(flowchart, blockID);

    public void FadeToBlack() =>
        GameManager.GLOBAL.fade.FadeToBlack();

    public void FadeFromBlack() =>
        GameManager.GLOBAL.fade.FadeFromBlack();

    public void FadeToWhite() =>
        GameManager.GLOBAL.fade.FadeToWhite();

    public void FadeFromWhite() =>
        GameManager.GLOBAL.fade.FadeFromWhite();
}
