using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable_Radio : InteractableWithDialogue
{
    public Interactable_Clock clock;

    public void CheckTime(string timeInput)
    {
        Debug.Log("Checking for input " + timeInput);

        string left = "";
        string right = "";

        for (int timeIndex = 0; timeIndex < 4; timeIndex++)
        {
            if (timeIndex < 2)
                left += timeInput[timeIndex];
            else
                right += timeInput[timeIndex];
        }

        int inputHours = int.Parse(left);
        int inputMinutes = int.Parse(right);

        // Get time now for easier refactoring
        DateTime currentTime = clock.GetTime();
        int hour12 = currentTime.Hour;


        bool isCorrect = false;

        // Check for all possible 12 hour shifts
        if (inputHours == hour12 || inputHours == hour12 - 12 || inputHours == hour12 + 12)
            if (inputMinutes == currentTime.Minute)
                isCorrect = true;

        if (!isCorrect)
        {
            TriggerDialogue("Wrong");
            return;
        }

        CorrectTime();
    }

    void CorrectTime()
    {
        Debug.Log("time correct");
        flowchart.SetBooleanVariable("isSolved", true);
        TriggerDialogue();
    }
}
