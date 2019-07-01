using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable_Clock : InteractableWithDialogue
{
    [Range(0, 23)]
    public int startHours = 12;

    [Range(0, 59)]
    public int startMinutes = 00;

    DateTime time;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        ChangeTime(startHours, startMinutes);
    }

    public void ChangeTime(int hours, int minutes)
    {
        time = new DateTime(2019, 07, 17, hours, minutes, 00);
    }

    public void AddMinutes(int minutes)
    {
        time = time.AddMinutes(minutes);
    }

    public override void Interact()
    {
        flowchart.SetStringVariable("currentTime", time.ToShortTimeString());

        base.Interact();
    }

    public DateTime GetTime() => time;
}
