using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Puzzle_Xylophone : Puzzle
{
    // public string solution = "10010110";
    bool[] solutionArray;
    public bool isPlaying = false;

    Interactable_XylophoneKey[] keys = new Interactable_XylophoneKey[8];

    private void OnEnable()
    {
        EventManager.Global.StartListening("Puzzle_Xylophone", EventFunction);
    }

    public void EventFunction(GameObject sender, string parameter = "")
    {
        Interactable_XylophoneStick stick = sender.GetComponent<Interactable_XylophoneStick>();

        if (stick)
            PuzzleCheck();
    }

    protected override void StartFunctions()
    {
        base.StartFunctions();

        keys = GetComponentsInChildren<Interactable_XylophoneKey>();

        solutionArray = CreateSolutionArray();
    }

    bool[] CreateSolutionArray()
    {
        bool[] output = new bool[keys.Length];
        int index = 0;

        foreach (char letter in solution.ToCharArray())
        {
            output[index] = (letter == '1');
            index++;
        }

        return output;
    }

    public override bool PuzzleCheck()
    {
        base.PuzzleCheck();

        for (int keyIndex = 0; keyIndex < keys.Length; keyIndex++)
        {
            if (solutionArray[keyIndex] != keys[keyIndex].selected)
            {
                Debug.Log("Wrong key at position " + keyIndex);
                PuzzleReset();
                return false;
            }
        }

        PuzzleSolved();
        return true;
    }

    public override void PuzzleReset()
    {
        base.PuzzleReset();

        for (int keyIndex = 0; keyIndex < keys.Length; keyIndex++)
            keys[keyIndex].Deselect();
    }
}
