﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class Interactable_XylophoneKey : Interactable
{
    public bool selected = false;

    float glowIntensity = 4f;

    int keyID = -1;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        colorLerpFactor = .016f;

        GetBookID();
    }

    public int GetBookID()
    {
        if (keyID >= 0)
            return keyID;

        string id = name;

        // Find integer number in GameObject name
        id = Regex.Match(id, @"\d+").Value;

        keyID = int.Parse(id);

        return keyID;
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();
    }

    /*
     * Prevent color glow change if book is selected for swap
     */
    public override void Focus()
    {
        if (selected)
            return;

        base.Focus();
    }

    public override void Unfocus()
    {
        if (selected)
            return;

        base.Unfocus();
    }

    public override void Interact()
    {
        if (!selected)
            Select();
        else
        {
            Deselect();
            Focus();
        }
    }

    void Select()
    {
        selected = true;
        SetGlowColor(meshRenderer.material.GetColor("_GlowColor"), isInstantTransition: true);
    }

    public void Deselect()
    {
        selected = false;
        SetGlowColor(Color.clear);
    }

    public void Play()
    {
        // Play sound

        selected = false;
        SetGlowColor(Color.clear);
    }
}
