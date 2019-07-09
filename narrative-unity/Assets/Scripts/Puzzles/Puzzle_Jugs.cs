using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Puzzle_Jugs : Puzzle
{
    public int solutionCapacity = 4;

    List<Interactable_Jug> jugs = new List<Interactable_Jug>();
    List<Interactable_Jug> swap = new List<Interactable_Jug>();

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        jugs = GetComponentsInChildren<Interactable_Jug>().ToList();
    }

    public void AddJug(Interactable_Jug jug)
    {
        swap.Add(jug);

        if (swap.Count >= 2)
            Refill();
    }

    public void RemoveJug(Interactable_Jug jug) => swap.Remove(jug);

    void Refill()
    {
        Debug.Log($"Refilling from {swap[0].maxCapacity} to {swap[1].maxCapacity}");

        // Empty first jug
        int move = swap[0].currentCapacity;
        swap[0].currentCapacity = 0;

        // Add capacity to second jug
        swap[1].currentCapacity += move;

        // Measure overfill
        int overfill = swap[1].currentCapacity - swap[1].maxCapacity;

        // If there is overfill, even it out
        if (overfill > 0)
        {
            swap[0].currentCapacity += overfill;
            swap[1].currentCapacity -= overfill;
        }

        foreach (Interactable_Jug jug in swap)
            jug.Refilled();

        swap.Clear();

        PuzzleCheck();
    }

    public override bool PuzzleCheck()
    {
        bool hasSolutionCapacity = false;

        foreach (Interactable_Jug jug in jugs)
            if (jug.currentCapacity == solutionCapacity)
                hasSolutionCapacity = true;

        if (hasSolutionCapacity)
            PuzzleSolved();

        return false;
    }
}
