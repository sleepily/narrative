using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Puzzle_Radio : Puzzle_CodeInput
{
    [Tooltip("Use clock's time as solution rather than a set one.")]
    public Interactable_Clock clock;

    public override bool PuzzleCheck(string playerInput) => CheckTime(playerInput);

    public bool CheckTime(string timeInput)
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
            return false;
        }

        return CorrectTime();
    }

    bool CorrectTime()
    {
        flowchart.SetBooleanVariable("isSolved", true);
        TriggerDialogue();
        return true;
    }
}
